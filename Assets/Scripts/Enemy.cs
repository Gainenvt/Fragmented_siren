using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int MaxHP = 100;
    public int MinHP = 0;

    public bool isSubmerged = false;
    public int CurrentHP;
    public bool isEnemyHit = false;

    public bool isEnemyDead = false;


    void Start()
    {
        CurrentHP = MaxHP;
    }


    void Update()
    {
        if (CurrentHP <= MinHP && !isEnemyDead)
        {
            isEnemyDead = true;
        }
        Dead();
    }


    private void Death(int amount)
    {
        if (isEnemyDead) return;

        CurrentHP -= amount;

        // Prevent HP from going below MinHP
        CurrentHP = Mathf.Max(CurrentHP, MinHP);

        Debug.Log("Enemy Health: " + CurrentHP);
    }


private void Dead()
{
    if (isEnemyDead)
    {
        Debug.Log("Enemy died!");
        Destroy(gameObject);
    }
}
}