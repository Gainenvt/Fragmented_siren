using UnityEngine;

public class Player : MonoBehaviour
{
    public int MaxHP = 100;
    public int MinHP = 0;

    public bool isSubmerged = false;
    public int CurrentHP;
    public bool isPlayerHit = false;
    public bool isPlayerDead = false;

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


    private void Death(int amount)
    {
        if (isPlayerDead) return;

        CurrentHP -= amount;

        // Min hp cap
        CurrentHP = Mathf.Max(CurrentHP, MinHP);

        Debug.Log("Player Health: " + CurrentHP);
    }


    private void Dead()
    {
        isPlayerDead = true;

        Debug.Log("Player has died!");

        UnlockCursor();

        deathScreen.SetActive(true);
    }
}