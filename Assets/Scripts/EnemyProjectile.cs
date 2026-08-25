using UnityEngine;
using UnityEngine.Rendering.Universal;


public class EnemyProjectile : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float moveSpeed = 10f;
    public int damage = 10;
    public float lifetime = 5f;
    public Player player;

    private Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        rb.linearVelocity = transform.forward * moveSpeed;
    }
    private void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0f)
        {
            Destroy(gameObject);
        }
    }
    private bool isPlayerHit = false;
    private void OnTriggerEnter(Collider other)
{
    if (isPlayerHit) return;

    Player player = other.GetComponent<Player>();
    Rigidbody rb = other.GetComponent<Rigidbody>();

    if (player != null)
    {
        isPlayerHit = true;

        player.CurrentHP -= damage;

        Debug.Log("Player hit! HP: " + player.CurrentHP);
    }

    Debug.Log("Hit object " + other.gameObject.name);
}

    }

