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


}
