using maxstAR;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int timerValue;
    public int scoreValue;
    [SerializeField] private Star[] stars;
    private TrackingState currentState;
    [SerializeField] ImageTrackableBehaviour track;
    private static GameController instance;
    private bool gameStarted;
    public static GameController Instance
    {
        get { return instance; }
    }
    void Start()
    {
        instance = this;
        

    }

    void Update()
    {
        OnTracking(track.TrackSuccess);
    }

    public void AddScore(int value)
    {
        scoreValue+= value;
        UIController.Instance.AddScore(scoreValue);
    }

    private void OnTracking(bool enable)
    {
        if (!gameStarted)
        {
            UIController.Instance.StartButton(enable);
        }
        if (!enable)
        {
            Time.timeScale = 0;
            SetColliders(false);
        }
    }

    internal void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("ver2");
    }

    public void ChangeGameState(bool state)
    {
        Time.timeScale = state ? 1 : 0;
        SetColliders(state);
    }
    public void StartGame()
    {
        StartCoroutine(Timer());
        gameStarted = true;
        foreach (var star in stars)
        {
            star.gameObject.SetActive(true);
        }
    }
    private void SetColliders(bool enable)
    {
        foreach (var star in stars)
        {
            star.SetClickable(enable);
        }
    }
    private IEnumerator Timer()
    {
        UIController.Instance.Timer(timerValue);
        if (timerValue <= 0) 
        {
            ChangeGameState(false);
            UIController.Instance.GameEnd();
            yield break;
            }
        timerValue--;
        yield return new WaitForSeconds(1);
        StartCoroutine(Timer());
    }
}
