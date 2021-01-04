using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObstacelBlock : MonoBehaviour, IInteractable
{
    #region FIELDS DECLERATION
    [Header("== REFERENCES ==")]
    public DataManager database;
    public GameObject spikes;

    [Header("== PROPERTIES ==")]
    public bool canDealDamage;
    public float force;
    public float radius;
    public float upwardModifier;

    // PRIVATE FIELDS DECLERATION
    private Rigidbody[] childCubes;

    #endregion

    private void OnEnable()
    {
        database.InvinsibleAction += PlayerBecomeInvinsible;
    }

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
        transform.GetChild(0).gameObject.SetActive(false);
        
        foreach (Rigidbody cube in childCubes)
        {
            cube.gameObject.SetActive(true);
        }

        foreach (Rigidbody cube in childCubes )
        {
            cube.AddExplosionForce(force, transform.position, radius, upwardModifier);
        }

        if (canDealDamage)
        {
            database.DealDamageInvoke();
            Vibrate.PerformeVibration(200);
        }
        else
        {
            AudioManager.Instance.PlaySFX(database.musicClips[3], 0.55f);
        }
    }

    /// <summary>
    /// If player become invinsible the don't do damage.
    /// </summary>
    private void PlayerBecomeInvinsible()
    {
        canDealDamage = false;
        spikes.SetActive(false);
    }

    private void OnDisable()
    {
        database.InvinsibleAction -= PlayerBecomeInvinsible;
    }

}
