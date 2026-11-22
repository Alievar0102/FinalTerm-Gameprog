using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameState : Singleton<GameState>
{
    public enum State
    {
        Playing, Paused, Rushed, GameOver, Win
    }
    public State currentState;

    public bool IsPlaying => currentState == State.Playing || currentState == State.Rushed;
    public bool IsRushed => currentState == State.Rushed;
    public bool IsPaused => currentState == State.Paused;
    public bool IsGameOver => currentState == State.GameOver;


    /*private float score;

    public TMP_Text scoreText;
    public float Score
    {
        get => score;
        set
        {
            score = value;
            if(scoreText != null)
            {
                scoreText.text = $"Score: {score:F5} BTC";
            }
            if (targetBar != null)
            {
                targetBar.DOFillAmount(score / target, 0.5f);
            }
        }
    }*/

    #region KillCount
    int killCount;

    public int KillCount
    {
        get
        {
            return killCount;
        }
        set
        {
            killCount = value;
        }
    }
    
    public TMP_Text killCountText;

    public void UpdateKillCount()
    {
        KillCount++;
        killCountText.text = GameState.Instance.KillCount.ToString();
    }
    #endregion



    public TMP_Text timeText;
    public float TimeLeft 
    { get => timeLeft;
        set
        {
            timeLeft = value;
            if(timeText != null)
            {
                timeText.text = $"Time: {timeLeft:F2}";
            }
        }
    }

    public float playTime = 60f;
    private float timeLeft;
    //public float rushTime = 0.5f;

    //public float target;
    //public Image targetBar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TimeLeft = playTime;
        currentState = State.Playing;
        //score = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsPlaying) return;

        TimeLeft -= Time.deltaTime;
        
        /*if(TimeLeft / playTime < rushTime)
        {
            currentState = State.Rushed;
        }*/

        if(TimeLeft < 0f)
        {
            currentState = State.Win;
            Time.timeScale = 0f;
        }
    }

    void OnPause()
    {
        switch(currentState)
        {
            case State.Playing:
                currentState = State.Paused;
                Time.timeScale = 0f;
                break;
            case State.Paused:
                currentState = State.Playing;
                Time.timeScale = 1f;
                break;
        }

        print(currentState.ToString());
    }
}
