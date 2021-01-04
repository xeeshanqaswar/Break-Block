using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region FIELDS DECLERATION

    [Header("== REFERENCES ==")]
    public DataManager database;
    public Transform player;

    [Header("== UI REFERENCES ==")]
    public Image levelProgressBar;
    public TextMeshProUGUI currentLvlText;
    public TextMeshProUGUI nextLevelText;
    public TextMeshProUGUI scoreCounterText;
    public Transform[] popupScreens;

    // PRIVATE FIELDS DECLERATION
    private int currentLevel;
    private int levelScore;
    private float totalDistance;
    private string calculateScore;

    #endregion

    private void OnEnable()
    {
        database.GameOverAction += GameFailEvent;
        database.ScoreBlockAction += UpdateScore;
        database.LevelCompleteAction += LevelComplete;
    }

    private void Start()
    {
        #region INITIALIZATIONS

        totalDistance = Vector3.Distance(LevelManager.Instance.levelContainer[currentLevel].startingPos.position,
                                         LevelManager.Instance.levelContainer[currentLevel].endingPos.position);
        currentLevel = database.currentLevel;
        levelScore = database.uLevelProps[currentLevel].levelScore;
        currentLvlText.text = currentLevel.ToString(); 
        nextLevelText.text = (currentLevel + 1).ToString();
        UpdateScore();

        #endregion

        OpenPopupScreen(2);
    }

    private void Update()
    {
        LevelProgression();
    }

    /// <summary>
    /// Update progress bar according to level progression
    /// </summary>
    private void LevelProgression()
    {
        float playerDistance = Vector3.Distance(player.transform.position, 
                                                LevelManager.Instance.levelContainer[currentLevel].endingPos.position);
        float distanceRatio = 1 - (playerDistance / totalDistance);
        levelProgressBar.fillAmount = distanceRatio + 0.03f;
    }

    private void OpenPopupScreen(int index)
    {
        for (int i = 0; i < popupScreens.Length; i++)
        {
            popupScreens[i].gameObject.SetActive(i == index);
        }
    }

    #region BUTTON EVENTS

    public void LevelStart()
    {
        AudioManager.Instance.PlaySFX(database.musicClips[2], 0.35f);
        database.GameStartInvoke();
    }
    
    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    #endregion

    #region SUBSCRIBED EVENTS

    private void LevelComplete()
    {
        if (currentLevel < database.maxlevels)
        {
            OpenPopupScreen(1);
            AudioManager.Instance.PlaySFX(database.musicClips[4], 0.6f);
            return;
        }

        OpenPopupScreen(3);
        AudioManager.Instance.PlaySFX(database.musicClips[4], 0.6f);
    }

    private void UpdateScore()
    {
        calculateScore = String.Concat(database.playerScore, " / ", levelScore);
        scoreCounterText.text = calculateScore;
    }

    private void GameFailEvent()
    {
        OpenPopupScreen(0);
    }

    #endregion

    private void OnDisable()
    {
        database.GameOverAction -= GameFailEvent;
        database.ScoreBlockAction -= UpdateScore;
        database.LevelCompleteAction -= LevelComplete;
    }

}
