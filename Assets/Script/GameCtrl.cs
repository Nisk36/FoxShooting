using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameCtrl : MonoBehaviour
{
    [SerializeField] private GameObject titleuiset = default;
    [SerializeField] private GameObject gameclearuiset = default;
    [SerializeField] private GameObject gameoveruiset = default;
    [SerializeField] private GameObject scoreuiset = default;
    [SerializeField] private GameObject resultuiset = default;
    [SerializeField] private GameObject bgm = default;
    [SerializeField] private GameObject bossbgm = default;


    public Text scoreText;
    public Text highScoreText;

    //ハイスコア保存
    private int highScore;

    private string highScoreKey = "highScore";

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
        bgm.SetActive(false);
        bossbgm.SetActive(false);
        gameProcList = new Dictionary<GameState, gameProc>
        {
            {GameState.TITLE, Title },
            {GameState.GAMEMAIN, GameMain },
            {GameState.CLEAR, Clear },
            {GameState.GAMEOVER, GameOver },
        };
        titleuiset.SetActive(true);
        gamestate = GameState.TITLE;
        ScoreInitialize();
    }

    void ScoreInitialize()
    {
        highScore = PlayerPrefs.GetInt(highScoreKey, 0);
    }

    // Update is called once per frame
    void Update()
    {
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
            StageCtrl.Instance.StageStart();
            bgm.SetActive(true);
            scoreuiset.SetActive(true);
            titleuiset.SetActive(false);
        }
    }

    private void GameMain()
    {
        if (StageCtrl.Instance.isBossAppear)
        {
            bgm.SetActive(false);
            bossbgm.SetActive(true);
        }
        if (!StageCtrl.Instance.isPlaying)
        {
            if (highScore < StageCtrl.Instance.GetScore()) 
            {
                highScore = StageCtrl.Instance.GetScore();
            }
            if(StageCtrl.Instance.playStopCode == StageCtrl.PlayStopCodeDef.PlayerDead)
            {
                Time.timeScale = 0;
                bgm.SetActive(false);
                bossbgm.SetActive(false);
                scoreuiset.SetActive(false);
                gameoveruiset.SetActive(true);
                resultuiset.SetActive(true);
                ShowResult();
                //naichilab.RankingLoader.Instance.SendScoreAndShowRanking(StageCtrl.Instance.GetScore());
                gamestate = GameState.GAMEOVER;
                highscoreSave();
                Debug.Log(highScore);
            }
            else
            {
                Time.timeScale = 0;
                bgm.SetActive(false);
                bossbgm.SetActive(false);
                scoreuiset.SetActive(false);
                gameclearuiset.SetActive(true);
                resultuiset.SetActive(true);
                ShowResult();
                //naichilab.RankingLoader.Instance.SendScoreAndShowRanking(StageCtrl.Instance.GetScore());
                gamestate = GameState.CLEAR;
                highscoreSave();
            }
        }
    }
    private void Clear()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            gamestate = GameState.TITLE;
            gameclearuiset.SetActive(false);
            resultuiset.SetActive(false);
            SceneManager.UnloadSceneAsync(1);
            titleuiset.SetActive(true);
            StageCtrl.Instance.ResetStage();
            Time.timeScale = 1;
        }
    }

    private void GameOver()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            gamestate = GameState.TITLE;
            gameoveruiset.SetActive(false);
            resultuiset.SetActive(false);
            titleuiset.SetActive(true);
            StageCtrl.Instance.ResetStage();
            Time.timeScale = 1;
        }
    }

    void highscoreSave()
    {
        PlayerPrefs.SetInt(highScoreKey, highScore);
        PlayerPrefs.Save();
    }

    int getHighScore()
    {
        return highScore;
    }

    void ShowResult()
    {
        scoreText.text = StageCtrl.Instance.GetScore().ToString();
        highScoreText.text = getHighScore().ToString();
    }
}
