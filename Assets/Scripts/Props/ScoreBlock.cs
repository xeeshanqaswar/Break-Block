using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreBlock : MonoBehaviour, IInteractable
{

    #region FIELDS DECLERATION

    [Header("== REFERENCES ==")]
    public DataManager database;

    [Header("== PROPERTIES ==")]
    public bool addScore;
    public float force;
    public float radius;
    public float upwardModifier;

    // PRIVATE FIELDS DECLERATION
    private Rigidbody[] childCubes;

    #endregion


    private void Start()
    {
        childCubes = new Rigidbody[transform.childCount - 1];
        for (int i = 0; i < childCubes.Length; i++)
        {
            childCubes[i] = transform.GetChild(i+1).GetComponent<Rigidbody>();
        }
    }

    public void Interact()
    {
        BreakMyself();
    }

    private void BreakMyself()
    {
        // Hide apparent block
        transform.GetChild(0).gameObject.SetActive(false);
        
        // Make Destroyable blocks visible
        foreach (Rigidbody cube in childCubes)
        {
            cube.gameObject.SetActive(true);
        }

        // Play SFX
        AudioManager.Instance.PlaySFX(database.musicClips[3], 0.55f);
        
        // Destroy
        foreach (Rigidbody cube in childCubes )
        {
            cube.AddExplosionForce(force, transform.position, radius, upwardModifier);
        }

        // Add score
        if (addScore) 
        {
            database.playerScore++;
            database.playerScore = Mathf.Clamp(database.playerScore, 0, database.uLevelProps[database.currentLevel].levelScore);
        }

        database.ScoreBlockInvoke();
    }
}
