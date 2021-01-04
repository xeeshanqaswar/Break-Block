using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompletePoint : MonoBehaviour,IInteractable
{
    #region FIELDS DECLERATION

    [Header("== REFERENCES ==")]
    public DataManager database;
    public LevelManager levelManager;
    public GameObject levelCompleteTrigger;
    public Animator entryGate;

    //[Header("== PROPERTIES ==")]

    //PRIVATE FIELDS DECLERATION
    private int currentLevel;
    private int scoreRequired;
    private bool canProceede = false;

    #endregion

    private void OnEnable()
    {
        database.ScoreBlockAction += OpenEntryGate;
    }

    private void Start()
    {
        currentLevel = database.currentLevel;
        scoreRequired = database.uLevelProps[currentLevel].levelScore;

    }

    public void OpenEntryGate()
    {
        // Open entry gate if player achieved the level score requirement;
        if (scoreRequired == database.playerScore)
        {
            entryGate.SetBool("OpenGate", true);
            canProceede = true;
        }
    }

    public void Interact()
    {
        levelCompleteTrigger.SetActive(false);

        if (!canProceede)
        {
            database.GameOverInvoke();
            AudioManager.Instance.PlaySFX(database.musicClips[0], 1f);
            Vibrate.PerformeVibration(250);
            return;
        }
        else
        {
            database.LevelCompleteInvoke();
            Vibrate.PerformeVibration(1000);
        }
    }

    private void OnDisable()
    {

        database.ScoreBlockAction -= OpenEntryGate;
    }

}
