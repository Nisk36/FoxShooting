using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject titleUiSet = default;
    [SerializeField] private GameObject gameClearUiSet = default;
    [SerializeField] private GameObject gameOverUiSet = default;
    [SerializeField] private GameObject scoreUiSet = default;
    [SerializeField] private GameObject resultUiSet = default;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text highScoreText;
    [SerializeField] private Text inGameScoreText; 
    public void InitializeUI()
    {
        titleUiSet.SetActive(false);
        scoreUiSet.SetActive(false);
        gameClearUiSet.SetActive(false);
        gameOverUiSet.SetActive(false);
        resultUiSet.SetActive(false);
    }

    public void SwitchUIView(GameCtrl.GameState state, int score, int highScore)
    {
        switch (state)
        {
            case GameCtrl.GameState.TITLE:
                InitializeUI();
                titleUiSet.SetActive(true);
                break;
            case GameCtrl.GameState.GAMEMAIN:
                titleUiSet.SetActive(false);
                scoreUiSet.SetActive(true);
                break;
            case GameCtrl.GameState.CLEAR:
                InitializeUI();
                gameClearUiSet.SetActive(true);
                resultUiSet.SetActive(true);
                ShowResult(score, highScore);
                break;
            case GameCtrl.GameState.GAMEOVER:
                InitializeUI();
                gameOverUiSet.SetActive(true);
                resultUiSet.SetActive(true);
                ShowResult(score, highScore);
                break;
        }
    }
    
    private void ShowResult(int score, int highScore)
    {
        scoreText.text = score.ToString();
        highScoreText.text = highScore.ToString();
    }

    public void SetInGameScore(int val)
    {
        inGameScoreText.text = $"{val:000000}";
    }
}

    
