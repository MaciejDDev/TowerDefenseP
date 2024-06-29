using Enemies;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TerrainGeneration;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("UI Menus")]
    [SerializeField] GameObject _mainMenu;
    [SerializeField] GameObject _inGameUI;
    [SerializeField] GameObject _howToPlayPanel;

    [Header("Player Tower")]
    [SerializeField] TowerAgent _towerAgent;

    [Header("HighScore Texts")]
    [SerializeField] TMP_Text _firstScoreText;
    [SerializeField] TMP_Text _secondScoreText;
    [SerializeField] TMP_Text _thirdScoreText;

    bool _isGamePaused;

    SelectionManager _selectionManager;
    GoldManager _goldManager;
    TileManager _tileManager;
    EnemyManager _enemyManager;
    BulletManager _bulletManager;
    TimerManager _timerManager;

    string firstScoreKey = "FirstScore";
    string secondScoreKey = "SecondScore";
    string thirdScoreKey = "ThirdScore";

    public bool IsGamePaused => _isGamePaused;

    

    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        _isGamePaused = true;
        
        _selectionManager = GetComponent<SelectionManager>();
        _selectionManager.Init();

        _goldManager = GetComponent<GoldManager>();
        _goldManager.Init();

        _tileManager = GetComponent<TileManager>();
        _tileManager.Init();

        _enemyManager = GetComponent<EnemyManager>();
        _enemyManager.Init();

        _bulletManager = GetComponent<BulletManager>();
        _bulletManager.Init();

        _timerManager = GetComponent<TimerManager>();
        _timerManager.Init();

        LoadGameData();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }



    private void LoadGameData()
    {
        if (CheckSavedScore(firstScoreKey))
            UpdateScoreText(firstScoreKey, _firstScoreText);
        if (CheckSavedScore(secondScoreKey))
            UpdateScoreText(secondScoreKey, _secondScoreText);
        if (CheckSavedScore(thirdScoreKey))
            UpdateScoreText(thirdScoreKey, _thirdScoreText);

    }

    private bool CheckSavedScore(string scoreKey)
    {
        bool exists = false;
        if (PlayerPrefs.HasKey(scoreKey))
            exists = true;

        return exists;
    }

    private void UpdateScoreText(string scoreKey, TMP_Text scoreText)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(PlayerPrefs.GetFloat(scoreKey));
        scoreText.text = string.Format("{0:00}:{1:00}", (int)timeSpan.TotalMinutes, (int)timeSpan.Seconds);
    }

    public void EndGame()
    {
        _selectionManager.TogglePlacementMenu(false);
        _mainMenu.SetActive(true);
        _inGameUI.SetActive(false);
        _isGamePaused = true;


        //Recycle Pools
        _selectionManager.RecycleAllPlaceables();
        _enemyManager.RecycleAllEnemies();
        _bulletManager.RecycleAllBullets();
        _tileManager.RecycleAllTiles();

        //Stop timer

        //Save highest Score
        SaveHighScore();
    }

    private void SaveHighScore()
    {
        //no hay datos
        if (!CheckSavedScore(firstScoreKey))
        {
            PlayerPrefs.SetFloat(firstScoreKey, _timerManager.TimerTime);
            UpdateScoreText(firstScoreKey, _firstScoreText);
            return;
        }

        var firstScoreSaved = PlayerPrefs.GetFloat(firstScoreKey);
        //se supera el 1º record
        if (_timerManager.TimerTime > firstScoreSaved)
        {
            PlayerPrefs.SetFloat(firstScoreKey, _timerManager.TimerTime);
            UpdateScoreText(firstScoreKey, _firstScoreText);

            if (!CheckSavedScore(secondScoreKey))
            {
                PlayerPrefs.SetFloat(secondScoreKey, firstScoreSaved);
                UpdateScoreText(secondScoreKey, _secondScoreText);
                return;
            }

            var secondScoreSaved = PlayerPrefs.GetFloat(secondScoreKey);

            PlayerPrefs.SetFloat(secondScoreKey, firstScoreSaved);
            UpdateScoreText(secondScoreKey, _secondScoreText);

            PlayerPrefs.SetFloat(thirdScoreKey, secondScoreSaved);
            UpdateScoreText(thirdScoreKey, _thirdScoreText);
            return;
        }


        if (!CheckSavedScore(secondScoreKey))
        {
            PlayerPrefs.SetFloat(secondScoreKey, _timerManager.TimerTime);
            UpdateScoreText(secondScoreKey, _secondScoreText);
            return;
        }

        var secondSavedScore = PlayerPrefs.GetFloat(secondScoreKey);
        if (_timerManager.TimerTime > secondSavedScore)
        {
            PlayerPrefs.SetFloat(secondScoreKey, _timerManager.TimerTime);
            UpdateScoreText(secondScoreKey, _secondScoreText);

            PlayerPrefs.SetFloat(thirdScoreKey, secondSavedScore);
            UpdateScoreText(thirdScoreKey, _thirdScoreText);
            return;
        }

        if (!CheckSavedScore(thirdScoreKey))
        {
            PlayerPrefs.SetFloat(thirdScoreKey, _timerManager.TimerTime);
            UpdateScoreText(thirdScoreKey, _thirdScoreText);
            return;
        }
        if (_timerManager.TimerTime > PlayerPrefs.GetFloat(thirdScoreKey))
        {
            PlayerPrefs.SetFloat(thirdScoreKey, _timerManager.TimerTime);
            UpdateScoreText(thirdScoreKey, _thirdScoreText);
            return;
        }
    }

    


    public void OnPlayButton()
    {
        _mainMenu.SetActive(false);
        _inGameUI.SetActive(true);
        _isGamePaused = false;
        //Start Game
        _tileManager.CreateNewLevel();
        
        //Reset Health
        _towerAgent.ResetHealth();
        //Reset Gold
        _goldManager.SetInitialGoldAmount();
        //ResetTimer
        _timerManager.Init();
    }

    IEnumerator FadePanel()
    {
        yield return null;
    }

    public void OnHowToPlayButton() 
    {
        _howToPlayPanel.SetActive(true);
    }

}
