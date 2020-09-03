using System;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private AudioSource tap;
    [SerializeField] private Button startButtom;
    [SerializeField] private Button resetButtom;
    [SerializeField] private Button pauseButtom;
    [SerializeField] private Text result;
    [SerializeField] private Text timerValue;
    [SerializeField] private Text scoreValue;
    [SerializeField] private GameObject popUp;

    private static UIController instance;
    public static UIController Instance
    {
        get { return instance; }
    }
    private void Awake()
    {
        AddScore(0);
        instance = this;
        startButtom.onClick.AddListener(StartGame);
        pauseButtom.onClick.AddListener(PauseButtom);
        resetButtom.onClick.AddListener(RestartGame);
        startButtom.onClick.AddListener(Tap);
        pauseButtom.onClick.AddListener(Tap);
        resetButtom.onClick.AddListener(Tap);
    }
    private void Tap()
    {
        tap.Play();
    }
    private void RestartGame()
    {
        GameController.Instance.RestartGame();
    }

    public void Timer(int timer)
    {
        timerValue.text = timer.ToString();
    }

    public void AddScore(int score)
    {
        scoreValue.text = score.ToString();
        result.text = score.ToString();

    }
    public void StartGame()
    {
        startButtom.gameObject.SetActive(false);
        GameController.Instance.StartGame();
        pauseButtom.gameObject.SetActive(true);
        timerValue.transform.parent.gameObject.SetActive(true);
        scoreValue.transform.parent.gameObject.SetActive(true);
        PauseButtom();
    }
    public void PauseButtom()
    {
            GameController.Instance.ChangeGameState(Time.timeScale == 0);
    }
    public void StartButton(bool show)
    {
        startButtom.gameObject.SetActive(show);
    }

    internal void GameEnd()
    {
        pauseButtom.onClick.RemoveAllListeners();
        popUp.SetActive(true);
    }
}
