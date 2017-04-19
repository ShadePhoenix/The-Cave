using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour {

    [Tooltip("Add the enemy to be spawned.")]
    public Transform enemy;
    [Tooltip("The time (in seconds) between enemies spawning.")]
    public float enemySpawnInterval = 3;
    private float timeUntilSpawn = 0;

    private GameObject[] spawners;
    private bool addedSpawners = false;

    // Use this for initialization
    void Start()
    {
       // GameObject[] tempSpawners;
        spawners = GameObject.FindGameObjectsWithTag("Spawners");
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

        PrepareSpawners();
    }

    void PrepareSpawners()
    {
        if(addedSpawners == false)
        {
            //spawner.s
        }
    }
}
