using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameCtrl : MonoBehaviour
{
    [SerializeField] 
    private UIManager uiManager = null;

    [SerializeField] 
    private StageCtrl stageCtrl = null;

    [SerializeField] 
    private BGMManager bgmManager = null;
    
    //ハイスコア保存
    private int highScore;
    private string highScoreKey = "highScore";
    //コールバックの結果受け取り用変数
    private bool isBoss;
    private bool isDead;
    private bool isClear;
    private int score;

    public enum GameState
    {
        TITLE,
        GAMEMAIN,
        CLEAR,
        GAMEOVER
    }
    private GameState gamestate = GameState.TITLE;

    delegate void gameProc();
    Dictionary<GameState, gameProc> gameProcList;
    // Start is called before the first frame update
    void Start()
    {
        if (uiManager != null)
        {
            uiManager.InitializeUI();
        }

        if (bgmManager != null)
        {
            bgmManager.InitializeBGM();
        }
        gameProcList = new Dictionary<GameState, gameProc>
        {
            {GameState.TITLE, Title },
            {GameState.GAMEMAIN, GameMain },
            {GameState.CLEAR, Clear },
            {GameState.GAMEOVER, GameOver },
        };
        ScoreInitialize();
        //コールバック登録
        stageCtrl.PlayerDeathHandler += DeathCallBackMethod;
        stageCtrl.GameClearHandler += ClearCallBackMethod;
        stageCtrl.BossAppearHandler += BossCallBackMethod;
        stageCtrl.HighScoreUpdateHandler += ScoreCallBackMethod;
        InitializeCallbackVaries();
    }

    void ScoreInitialize()
    {
        highScore = PlayerPrefs.GetInt(highScoreKey, 0);
    }

    // Update is called once per frame
    void Update()
    {
        uiManager.SwitchUIView(gamestate, score, highScore);
        gameProcList[gamestate]();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void Title()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            gamestate = GameState.GAMEMAIN;
            InitializeCallbackVaries();
            stageCtrl.StageStart();
            bgmManager.PlayNormalBGM();
        }
    }

    private void GameMain()
    {
        //ScoreUI
        uiManager.SetInGameScore(score);
        
        if (isBoss)
        {
            bgmManager.PlayBossBGM();
        }

        if (isClear)
        {
            Time.timeScale = 0;
            gamestate = GameState.CLEAR;
            highscoreSave();
            InitializeCallbackVaries();
        }

        if (isDead)
        {
            Time.timeScale = 0;
            gamestate = GameState.GAMEOVER;
            highscoreSave();
            InitializeCallbackVaries();
            Debug.Log(highScore);
        }

        if (highScore < score) 
        {
            highScore = score;
        }
    }
    private void Clear()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            gamestate = GameState.TITLE;
            bgmManager.InitializeBGM();
            stageCtrl.ResetStage();
            Time.timeScale = 1;
        }
    }

    private void GameOver()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            gamestate = GameState.TITLE;
            bgmManager.InitializeBGM();
            stageCtrl.ResetStage();
            Time.timeScale = 1;
        }
    }

    void highscoreSave()
    {
        PlayerPrefs.SetInt(highScoreKey, highScore);
        PlayerPrefs.Save();
    }

    void DeathCallBackMethod(string res)
    {
        Debug.Log(isDead);
        isDead = true;
    }

    void ClearCallBackMethod(string res)
    {
        isClear = true;
    }

    void BossCallBackMethod(string res)
    {
        isBoss = true;
    }

    void ScoreCallBackMethod(int res)
    {
        score = res;
    }

    void InitializeCallbackVaries()
    {
        isClear = false;
        isBoss = false;
        isDead = false;
        score = 0;
    }
    
    
}
