using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourSingleton<GameManager>
{
    [SerializeField, Range(0f,3f)] float sceneChangeDelay = 1f;

    [SerializeField] UnityEvent OnInit;
    [SerializeField] UnityEvent OnWin;
    [SerializeField] UnityEvent OnGameOver;

    [SerializeField] PlayerController playerController;

    public void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();

        try
        {
            OnInit.Invoke();
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    public void Win()
    {

        playerController.gameObject.GetComponentInChildren<Animator>().Play("Victory");

        try
        {
            OnWin.Invoke();
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }

        StartCoroutine(_Win());
    }

    private IEnumerator _Win()
    {
        yield return new WaitForSecondsRealtime(sceneChangeDelay);
        LevelManager.Instance.GoToNextScene();
    }

    public void GameOver()
    {
        Debug.Log("GameOver");

        try
        {
            OnGameOver.Invoke();
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }

        StartCoroutine(_GameOver());
    }

    private IEnumerator _GameOver() { 
        yield return new WaitForSecondsRealtime(sceneChangeDelay);
        LevelManager.Instance.RestartScene();
    }
}
