using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Database",menuName ="DataManager")]
public class DataManager : ScriptableObject
{
    [Header("== LEVEL PROPERTIES ==")]
    public int currentLevel;
    public int unLockedlevels;
    public int maxlevels;

    [Header("== GENERAL DATA ==")]
    public int playerScore;
    public UniversalLevelProperties[] uLevelProps;
    public AudioClip[] musicClips;

    #region GAME EVENTS

    public event Action GameStartAction;
    public event Action GameOverAction;
    public event Action LevelCompleteAction;
    public event Action ScoreBlockAction;
    public event Action DealDamageAction;
    public event Action InvinsibleAction;

    #endregion

    #region GAME EVENTS INVOCATION

    public void GameStartInvoke()
    {
        GameStartAction?.Invoke();
    }

    public void GameOverInvoke()
    {
        GameOverAction?.Invoke();
    }

    public void LevelCompleteInvoke()
    {
        LevelCompleteAction?.Invoke();
    }

    public void ScoreBlockInvoke()
    {
        ScoreBlockAction?.Invoke();
    }

    public void DealDamageInvoke()
    {
        DealDamageAction?.Invoke();
    }

    public void InvinsibleInvoke()
    {
        InvinsibleAction?.Invoke();
    }

    #endregion

    public void ResetData() 
    {
        playerScore = 0;
    }

}

[Serializable]
public class UniversalLevelProperties{

    public string name;
    public int levelScore;
    public float levelTime;
    public Color levelSky;
}
