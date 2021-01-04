using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerDestroyed : MonoBehaviour
{
    #region FIELDS DECLERATION
    public Transform destrucableObj;

    [Header("== PROPERTIES ==")]
    public float force;
    public float radius;
    public float upwardModifier;

    // PRIVATE FIELDS DECLERATION
    private Rigidbody[] childCubes;

    #endregion


    private void Start()
    {
        childCubes = new Rigidbody[destrucableObj.childCount];
        for (int i = 0; i < childCubes.Length; i++)
        {
            childCubes[i] = destrucableObj.GetChild(i).GetComponent<Rigidbody>();
        }
    }

    public void DestroyPlayer()
    {
        foreach (Rigidbody cube in childCubes)
        {
            cube.gameObject.SetActive(true);
        }

        foreach (Rigidbody cube in childCubes )
        {
            cube.AddExplosionForce(force, transform.position, radius, upwardModifier);
        }
    }
}
