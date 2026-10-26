
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public string mainMenuSceneName = "MainMenu";

    private bool isPaused = false;
    private InputAction pauseAction;

    void Awake()
    {
        pauseAction = new InputAction(
            type: InputActionType.Button,
            binding: "<Keyboard>/escape"
        );

        pauseAction.performed += OnPausePressed;
    }

    void OnEnable()
    {
        pauseAction.Enable();
    }

    void OnDisable()
    {
        pauseAction.Disable();
    }

    void OnDestroy()
    {
        pauseAction.performed -= OnPausePressed;
        pauseAction.Dispose();
    }

    void Start()
    {
        Time.timeScale = 1f;

        if (pauseMenu != null)
            pauseMenu.SetActive(false);

        isPaused = false;
    }

    private void OnPausePressed(InputAction.CallbackContext context)
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        isPaused = true;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);

        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isPaused = false;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();

        Debug.Log(
            "Quit Game pressed. In the Unity Editor, the game won't close."
        );
    }
}
