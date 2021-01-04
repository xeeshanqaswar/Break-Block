using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    #region FIELDS DECLERATION

    [Header("== REFERENCES ==")]
    public DataManager database;
    public Transform player;

    [Header("== LEVEL PROPERTIES ==")]
    public LevelProperty[] levelContainer;

    // Private Fields decleration
    private int currentLevel;
    private Camera mainCamera;

    #region STATIC INSTANCE

    private static LevelManager instance;
    public static LevelManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("LevelManager Doesn't exist");
                return null;
            }
            return instance;
        }
        private set
        {
            instance = value;
        }
    }

    #endregion

    #endregion

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        database.ResetData();
        database.LevelCompleteAction += LevelComplete;
    }

    private void Start()
    {
        currentLevel = database.currentLevel;
        mainCamera = Camera.main.GetComponent<Camera>();
        InitializeLevel();
    }

    private void InitializeLevel()
    {
        for (int i = 0; i < levelContainer.Length; i++)
        {
            levelContainer[i].sceneObject.SetActive(i == currentLevel);
            if (i == currentLevel) { 
                player.transform.position = levelContainer[i].startingPos.position;
                mainCamera.backgroundColor = database.uLevelProps[i].levelSky;
                RenderSettings.fogColor = database.uLevelProps[i].levelSky;
            }
        }
    }

    private void LevelComplete()
    {
        database.currentLevel++;
        database.currentLevel = Mathf.Clamp(database.currentLevel, 0, database.maxlevels);
        if (database.currentLevel > database.maxlevels)
        {
            database.maxlevels = database.currentLevel;
        }
    }

    private void OnDisable()
    {
        database.LevelCompleteAction -= LevelComplete;
    }

}

[Serializable]
public class LevelProperty
{
    public string name;
    public Transform startingPos;
    public Transform endingPos;
    public GameObject sceneObject;
}

