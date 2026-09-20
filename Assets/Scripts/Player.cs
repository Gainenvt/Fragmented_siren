using UnityEngine;

public class Player : MonoBehaviour
{
    public float MaxHP = 100;
    public float MinHP = 0;

    public bool isSubmerged = false;
    public float CurrentHP;
    public bool isPlayerHit = false;
    public bool isPlayerDead = false;
    public bool isPlayerInvincible = false;
    public float invincibilityDuration = 1f;
    public bool isDashing = false;

    [SerializeField] private GameObject deathScreen;


    void Start()
    {
        CurrentHP = MaxHP;
        LockCursor();

        deathScreen.SetActive(false);
    }


    void Update()
    {
        if (CurrentHP <= MinHP && !isPlayerDead)
        {
            Dead();
        }
    }


    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }


  public void TakeDamage(int amount)
    {
        if (isPlayerDead) return;

        CurrentHP -= amount;

        CurrentHP = Mathf.Max(CurrentHP, MinHP);

        Debug.Log("Player Health: " + CurrentHP);

        if(CurrentHP <= MinHP)
        {
            isPlayerDead = true;
            Debug.Log("Player has died!");
            Dead();
        }
    }

    private void Dead()
    {
        isPlayerDead = true;

        Debug.Log("Player has died!");

        UnlockCursor();

        deathScreen.SetActive(true);
    }
}