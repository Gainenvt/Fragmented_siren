using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public string mainMenuSceneName = "MainMenu";

    [Header("Gameplay")]
    [SerializeField] private PlayerMovement playerMovement;

    private PlayerInputActions input;
    private bool isPaused = false;

    private void Awake()
    {
        input = new PlayerInputActions();
    }

    private void OnEnable()
    {
        if (input == null)
            input = new PlayerInputActions();

        input.Player.Pause.performed += OnPause;
        input.Enable();
    }

    private void OnDisable()
    {
        if (input == null)
            return;

        input.Player.Pause.performed -= OnPause;
        input.Disable();
    }

    private void Start()
    {
        Time.timeScale = 1f;
        isPaused = false;

        if (pauseMenu != null)
            pauseMenu.SetActive(false);

        if (playerMovement == null)
            playerMovement = FindFirstObjectByType<PlayerMovement>();

        if (playerMovement != null)
            playerMovement.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        if (isPaused)
            return;

        if (pauseMenu != null)
            pauseMenu.SetActive(true);

        if (playerMovement != null)
            playerMovement.enabled = false;

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isPaused = true;
    }

    public void ResumeGame()
    {
        if (pauseMenu != null)
            pauseMenu.SetActive(false);

        Time.timeScale = 1f;

        if (playerMovement != null)
            playerMovement.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isPaused = false;
    }
}
