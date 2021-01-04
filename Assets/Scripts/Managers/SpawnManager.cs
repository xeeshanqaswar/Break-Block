using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    #region FIELDS DECLERATION
    
    [Header("== REFERENCES ==")]
    public GameObject blockPrefabs;

    [Header("== PROPERTIES ==")]
    public bool countChance;
    public int spawnCount;
    public Vector3 initialOffset;
    public float distanceBtw;
    public float hThreshold;

    // PRIVATE FIELDS DECLERATION
    private Vector3 spawnPosition;
    private GameObject parentObject;

    #endregion

    void Start()
    {
        parentObject = new GameObject(blockPrefabs.name + "Parent");
        parentObject.transform.parent = transform;

        spawnPosition = initialOffset;
        SpawnScoreCubes();
    }

    private void SpawnScoreCubes()
    {
        for ( int i = 0; i< spawnCount; i++)
        {
            spawnPosition += Vector3.forward * distanceBtw;
            spawnPosition.x = UnityEngine.Random.Range(-hThreshold, hThreshold);
            Instantiate(blockPrefabs, spawnPosition, Quaternion.identity, parentObject.transform);
        }
    }

    private bool ChanceCount()
    {
        int randNumb = UnityEngine.Random.Range(0, 5);
        return randNumb == 2;
    }
}
