using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField, Range(0f,3f)] float sceneChangeDelay = 1f;

    public void Win()
    {

    }

    public void GameOver()
    {
        Debug.Log("GameOver");
        StartCoroutine(_GameOver());
    }

    private IEnumerator _GameOver() { 
        yield return new WaitForSeconds(sceneChangeDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
