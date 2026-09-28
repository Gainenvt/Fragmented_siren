using UnityEngine;
using UnityEngine.InputSystem;

public class Ripple : MonoBehaviour
{   [SerializeField] private float rippleDuration = 1f;
    [SerializeField] private Material waterMaterial;



public float rippleTimer = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RipplePoint(MouseLocation());
         Debug.Log("Mouse Position: " + MouseLocation());
        
        {
            rippleTimer += Time.deltaTime;

            if (rippleTimer >= rippleDuration)
            {
               rippleTimer = 0f;
            }
        }

        
    }
    void FixedUpdate()
    {
      

    }
    
   private Vector2 MouseLocation()
{
    Vector2 mousePosition = Mouse.current.position.ReadValue();

    mousePosition.x /= Screen.width;
    mousePosition.y /= Screen.height;

    return mousePosition;
}

private void RipplePoint(Vector2 point)
{
    waterMaterial.SetVector("_RipplePoint", new Vector4(point.x, point.y, 0f, 0f));
}
}
