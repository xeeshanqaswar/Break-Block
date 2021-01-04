using DitzeGames.MobileJoystick;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerBehaviour : MonoBehaviour
{

    #region FIELDS DECLERATION

    [Header("== REFERENCES ==")]
    public DataManager database;
    public TouchField touchField;
    public GameObject smallCube;
    public GameObject bigCube;
    public Transform powerUp;

    [Header("== PROPERTIES ==")]
    public float playerHealth;
    public float playerSpeed;
    public float horizontalThreshold = 2.8f;

    //Private fields decleration
    private bool canRun = false;
    private float horizontal = 0f;
    private int playerScore;
    private float levelScore;
    private Rigidbody mRigidbody;
    private PlayerDestroyed playerDestroyed;
    private bool invinsible = false;
    private bool triggerEnterBuffer = true;

    #endregion

    private void OnEnable()
    {
        database.GameStartAction += GameStartEvent;
        database.GameOverAction += GameFailEvent;
        database.LevelCompleteAction += GameCompleteEvent;
        database.ScoreBlockAction += PlayerPowerUp;
        database.DealDamageAction += ReceiveDamage;
    }

    private void Start()
    {
        mRigidbody = GetComponent<Rigidbody>();
        playerDestroyed = GetComponent<PlayerDestroyed>();
        levelScore = database.uLevelProps[database.currentLevel].levelScore;
    }

    private void Update()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        if (canRun)
        {
            horizontal = touchField.TouchDist.x;
            transform.Translate(new Vector3(horizontal, 0f, 1f * playerSpeed) * Time.deltaTime);

            Vector3 pos = transform.position;
            pos.x = Mathf.Clamp(pos.x, -horizontalThreshold, horizontalThreshold);
            transform.position = pos;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IInteractable interactable = other.transform.parent.gameObject.GetComponent<IInteractable>();
        if (interactable != null)
        {
            interactable.Interact();
        }
    }

    private void PlayerPowerUp()
    {
        if (playerScore == (levelScore - 1))
        {
            smallCube.SetActive(false);
            bigCube.SetActive(true);
            invinsible = true;
            database.InvinsibleInvoke();
            Vibrate.PerformeVibration(250);
        }
        else
        {
            playerScore = database.playerScore;
            float scoreRatio = playerScore / levelScore;
            if (scoreRatio < 1.01f)
            {
                powerUp.localScale = new Vector3(1.01f, scoreRatio + 0.01f, 1.01f);
            }
        }
    }

    #region EVENTS SUBSCRIBERS

    private void GameStartEvent()
    {
        canRun = true;
    }

    private void GameCompleteEvent()
    {
        Invoke("StopPlayer", 0.75f);
    }

    private void StopPlayer()
    {
        canRun = false;
    }

    private void GameFailEvent()
    {
        canRun = false;
        smallCube.SetActive(false);
        powerUp.gameObject.SetActive(false);
        playerDestroyed.DestroyPlayer();
    }

    private void ReceiveDamage()
    {
        if (playerHealth <= 0)
        {
            AudioManager.Instance.PlaySFX(database.musicClips[0], 1f);
            database.GameOverInvoke();
            return;
        }

        AudioManager.Instance.PlaySFX(database.musicClips[5], 0.4f);
        playerHealth--;
    }

    #endregion

    private void OnDisable()
    {
        database.GameStartAction -= GameStartEvent;
        database.GameOverAction -= GameFailEvent;
        database.LevelCompleteAction -= GameCompleteEvent;
        database.ScoreBlockAction -= PlayerPowerUp;
        database.DealDamageAction -= ReceiveDamage;
    }

}
