
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basesiren : MonoBehaviour
{
    public int MaxHP = 100;
    public int MinHP = 0;
    public int CurrentHP;

    [SerializeField] private ParticleSystem damageParticle;

    [Header("Damage Flash")]
    [SerializeField] private Color damageColor = Color.red;
    [SerializeField] private float flashDuration = 0.15f;

    private readonly List<Material> enemyMaterials = new List<Material>();
    private readonly List<Color> originalColors = new List<Color>();
    private Coroutine flashCoroutine;

    public bool isEnemyDead = false;

    private void Start()
    {
        CurrentHP = MaxHP;

        Renderer[] renderers =
            GetComponentsInChildren<Renderer>();

        foreach (Renderer rend in renderers)
        {
            
            foreach (Material mat in rend.materials)
            {
                string colorProperty = GetColorProperty(mat);

                if (colorProperty == null)
                    continue;

                enemyMaterials.Add(mat);
                originalColors.Add(mat.GetColor(colorProperty));
            }
        }

        Debug.Log("Enemy spawned with HP: " + CurrentHP);
    }

    public void TakeDamage(int amount)
    {
        if (isEnemyDead) return;

        Debug.Log("Enemy took damage: " + amount);

        CurrentHP -= amount;
        CurrentHP = Mathf.Max(CurrentHP, MinHP);

        Debug.Log("Enemy Health: " + CurrentHP);

        PlayDamageEffect();

        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }

        flashCoroutine = StartCoroutine(FlashRed());

        if (CurrentHP <= MinHP)
        {
            Dead();
        }
    }

    private void PlayDamageEffect()
    {
        if (damageParticle != null)
        {
            damageParticle.Play();
        }
    }

    private IEnumerator FlashRed()
    {
        SetEnemyColor(damageColor);

        yield return new WaitForSeconds(flashDuration);

        RestoreOriginalColor();

        flashCoroutine = null;
    }

    private void SetEnemyColor(Color color)
    {
        for (int i = 0; i < enemyMaterials.Count; i++)
        {
            Material mat = enemyMaterials[i];
            string property = GetColorProperty(mat);

            if (property != null)
            {
                mat.SetColor(property, color);
            }
        }
    }

    private void RestoreOriginalColor()
    {
        for (int i = 0; i < enemyMaterials.Count; i++)
        {
            Material mat = enemyMaterials[i];
            string property = GetColorProperty(mat);

            if (property != null)
            {
                mat.SetColor(property, originalColors[i]);
            }
        }
    }

    private string GetColorProperty(Material mat)
    {
        if (mat.HasProperty("_BaseColor"))
            return "_BaseColor";

        if (mat.HasProperty("_Color"))
            return "_Color";

        return null;
    }

    private void Dead()
    {
        isEnemyDead = true;

        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
            flashCoroutine = null;
        }

        RestoreOriginalColor();

        Debug.Log("Enemy is dead! Current HP: " + CurrentHP);

        gameObject.SetActive(false);
    }
}
