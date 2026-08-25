using UnityEngine;

public class PuzzelScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform CircleHole;


 private void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Circle"))
    {
        Destroy(other.gameObject); // circle
        Destroy(transform.parent.gameObject); // cylinder parent
    }
}
}
