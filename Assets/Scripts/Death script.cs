using UnityEngine;
using UnityEngine.SceneManagement;
public class Deathscript : MonoBehaviour
{       [SerializeField] private string GameSceneMainMenu;

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void pause()
    {
        Time.timeScale = 0f;
        GetComponent<PlayerMovement>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pause();
        }
    }

}
