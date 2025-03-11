using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviourSingleton<LevelManager>
{
    public string mainMenuScene = "";
    public string nextScene = "";

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }

    public void GoToNextScene()
    {
        SceneManager.LoadScene(nextScene);
    }

    public void GoToScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
