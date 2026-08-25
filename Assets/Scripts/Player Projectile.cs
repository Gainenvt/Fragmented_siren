using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public int DMG = 10;
    public int SPD = 10;
    public float lifetime = 5f;
    public bool isEnemyHit = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
   private void Update()
    {
        lifetime -= Time.deltaTime;

        if (lifetime <= 0f)
        {
            Destroy(gameObject);
        }
    }

     private void OnTriggerEnter(Collider other)
    {
        if (isEnemyHit) return;

        Enemy enemy = other.GetComponent<Enemy>();

        if (enemy != null)
        {
            isEnemyHit = true;

            enemy.CurrentHP -= DMG;

            Debug.Log("Enemy hit! HP: " + enemy.CurrentHP);

            Destroy(gameObject);
        }

        Debug.Log("Hit object: " + other.gameObject.name);
    }

}
