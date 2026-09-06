using UnityEngine;

public class Basesiren : MonoBehaviour
{
    public int MaxHP = 100;
    public int MinHP = 0;
    public int CurrentHP;

    public bool isEnemyDead = false;

    private void Start()
    {
        CurrentHP = MaxHP;

        Debug.Log("Enemy spawned with HP: " + CurrentHP);
    }

    public void TakeDamage(int amount)
    {
        Debug.Log("Enemy took damage: " + amount);

        if (isEnemyDead) return;

        CurrentHP -= amount;

        CurrentHP = Mathf.Max(CurrentHP, MinHP);

        Debug.Log("Enemy Health: " + CurrentHP);

        if (CurrentHP <= MinHP)
        {
            isEnemyDead = true;
            Dead();
        }
    }

    private void Dead()
    {
        Debug.Log("Enemy is dead!");

        Destroy(gameObject);
    }
}