﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour {

    [Tooltip("Add the enemy to be spawned.")]
    public Transform enemy;
    [Tooltip("The time (in seconds) between enemies spawning.")]
    public float enemySpawnInterval = 3;
    private float timeUntilSpawn = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timeUntilSpawn -= Time.deltaTime;
        if (timeUntilSpawn <= 0)
        {
            Instantiate(enemy, transform.position, Quaternion.identity);
            timeUntilSpawn = enemySpawnInterval;
        }
    }
}
