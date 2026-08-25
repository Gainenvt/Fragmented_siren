using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    
    private PlayerInputActions input;
    private Rigidbody rb;
    private Player player;

    private Vector2 moveInput;
    private Vector2 lookInput;

    private float xRotation = 0f;

    private bool isDescending;
    private bool isAscending;



    // GRAB SYSTEM

    // GRAB SYSTEM

private GameObject selectedObject;

public float grabHeight = 0.25f;

private float grabDistance;

public float scrollSpeed = 2f;
public float minGrabDistance = 1f;
public float maxGrabDistance = 10f;
[SerializeField] private float verticalOffset = 0f;

   
    // MOVEMENT SETTINGS
    

    public float JumpForce = 2f;
    public float moveSPD = 3f;
    public float swimSpd = 2f;

    
    // LOOK SETTINGS
   

    public float LookSpeed = 15f;
    public float ControllerLookSPD = 100f;

    private float currentLookSpeed;

    // Refs

    public Transform PlayerCamera;

    public GameObject projectilePrefab;
    public Transform spawnPoint;

    // INITIALIZATION 

    private void Awake()
    {
        input = new PlayerInputActions();

        rb = GetComponent<Rigidbody>();
        player = GetComponent<Player>();

        currentLookSpeed = LookSpeed;
    }

    // INPUT SYSTEM

    private void OnEnable()
    {
        if (input == null)
        {
            input = new PlayerInputActions();
        }

        input.Enable();

        input.Player.Move.performed += OnMove;
        input.Player.Move.canceled += OnMove;

        input.Player.Look.performed += OnLook;
        input.Player.Look.canceled += OnLook;

        input.Player.Jump.performed += OnJump;
        input.Player.Jump.canceled += OnJump;

        input.Player.Descend.performed += OnDescend;
        input.Player.Descend.canceled += OnDescend;

        input.Player.SpearATK.performed += OnSpearATK;

        input.Player.Grab.performed += OnGrab;
        input.Player.RotateGrabbed.performed += OnRotateGrabbed;
    }

    private void OnDisable()
    {
        if (input == null)
            return;

        input.Player.Move.performed -= OnMove;
        input.Player.Move.canceled -= OnMove;

        input.Player.Look.performed -= OnLook;
        input.Player.Look.canceled -= OnLook;

        input.Player.Jump.performed -= OnJump;
        input.Player.Jump.canceled -= OnJump;

        input.Player.Descend.performed -= OnDescend;
        input.Player.Descend.canceled -= OnDescend;

        input.Player.SpearATK.performed -= OnSpearATK;

        input.Player.Grab.performed -= OnGrab;
        input.Player.RotateGrabbed.performed -= OnRotateGrabbed;

        input.Disable();
    }

    // UPDATE

    private void Update()
    {
        Look();
        Swimming();
        GrabObject();
        ScrollGrabbedObject();

    }

    private void FixedUpdate()
    {
        Move();
    }

    // MOVEMENT INPUT

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    // LOOK INPUT

    private void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();

        if (context.control.device is Mouse)
        {
            currentLookSpeed = LookSpeed;
        }
        else if (context.control.device is Gamepad)
        {
            currentLookSpeed = ControllerLookSPD;
        }
    }

    // JUMP INPUT

    private void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isAscending = true;
        }

        if (context.canceled)
        {
            isAscending = false;
        }
    }

    // DESCEND INPUT

    private void OnDescend(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isDescending = true;
        }

        if (context.canceled)
        {
            isDescending = false;
        }
    }

    // SPEAR ATTACK

    private void OnSpearATK(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameObject projectile = Instantiate(
                projectilePrefab,
                spawnPoint.position,
                spawnPoint.rotation
            );

            SpearATK(projectile);
        }
    }

    private void SpearATK(GameObject projectile)
    {
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        rb.linearVelocity = spawnPoint.forward * 10f;
    }

    // GRAB INPUT

    private void OnGrab(InputAction.CallbackContext context)
    {
        if (selectedObject == null)
        {
            Camera camera = PlayerCamera.GetComponent<Camera>();

            Ray ray = camera.ScreenPointToRay(
                Mouse.current.position.ReadValue()
            );

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (!hit.collider.CompareTag("drag"))
                {
                    return;
                }

                selectedObject = hit.collider.gameObject;

            
            grabDistance = Vector3.Distance(
            camera.transform.position,
            selectedObject.transform.position);

                Cursor.visible = false;
            }
        }
        else
        {
            selectedObject = null;

            Cursor.visible = true;
        }
    }

    private void OnRotateGrabbed(InputAction.CallbackContext context)
    {
        if (selectedObject != null)
        {
            selectedObject.transform.Rotate(
                0f,
                90f,
                0f
            );
        }
    }

    // GRAB MOVEMENT

   private void GrabObject()
{
    if (selectedObject == null)
    {
        return;
    }

    Camera camera = PlayerCamera.GetComponent<Camera>();

    // Mouse wheel adjusts its position above/below the camera's center.
    verticalOffset += Mouse.current.scroll.ReadValue().y * scrollSpeed;

    Vector3 heldPosition =
        camera.transform.position +
        camera.transform.forward * grabDistance +
        camera.transform.up * verticalOffset;

    selectedObject.transform.position = heldPosition;
}

    // Move

    private void Move()
    {
        Vector3 movement;

        if (player.isSubmerged)
        {
            movement =
                PlayerCamera.right * moveInput.x +
                PlayerCamera.forward * moveInput.y;
        }
        else
        {
            movement =
                transform.right * moveInput.x +
                transform.forward * moveInput.y;
        }

        float verticalVelocity = rb.linearVelocity.y;

        if (player.isSubmerged)
        {
            verticalVelocity = movement.y * moveSPD;
        }

        if (isAscending)
        {
            verticalVelocity = swimSpd;
        }
        else if (isDescending)
        {
            verticalVelocity = -swimSpd;
        }

        rb.linearVelocity = new Vector3(
            movement.x * moveSPD,
            verticalVelocity,
            movement.z * moveSPD
        );
    }

    // LOOK

    private void Look()
    {
        transform.Rotate(
            Vector3.up *
            lookInput.x *
            currentLookSpeed *
            Time.deltaTime
        );

        xRotation -=
            lookInput.y *
            currentLookSpeed *
            Time.deltaTime;

        xRotation = Mathf.Clamp(
            xRotation,
            -90f,
            90f
        );

        PlayerCamera.localRotation =
            Quaternion.Euler(
                xRotation,
                0f,
                0f
            );
    }

    // SWIMMING

    private void Swimming()
    {
        if (player.isSubmerged)
        {
            rb.useGravity = false;
        }
        else
        {
            rb.useGravity = true;
        }
    }

    private void ScrollGrabbedObject()
{
    if (selectedObject == null)
    {
        return;
    }

    float scroll = Mouse.current.scroll.ReadValue().y;

    grabDistance -= scroll * scrollSpeed * Time.deltaTime;

    grabDistance = Mathf.Clamp(
        grabDistance,
        minGrabDistance,
        maxGrabDistance
    );
}
}