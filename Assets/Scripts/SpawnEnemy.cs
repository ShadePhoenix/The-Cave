using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{

    [Tooltip("Add the enemy to be spawned.")]
    public Transform enemy;
    [Tooltip("The time (in seconds) between enemies spawning.")]
    public float enemySpawnInterval = 3;
    [Tooltip("The maximum number of enemies that can spawn on this level.")]
    public int maxEnemiesToSpawn = 2;
    [Tooltip("The minimum number of enemies that can spawn on this level.")]
    public int minEnemiesToSpawn = 1;

    private float timeUntilSpawn = 0;
    private GameObject[] spawners;   
    private bool spawning = false;
    
    void Start()
    {        
        spawners = GameObject.FindGameObjectsWithTag("Spawner");
        StartCoroutine("Spawn");
    }

    void Update() { }

    private IEnumerator Spawn()
    {
        while(true)
        {
            spawning = true;
            int rSpawn = Random.Range(0, spawners.Length);            
            StartCoroutine("SpawnNow", rSpawn);            
            yield return new WaitForSeconds(3);
        }         
    }
   
    private IEnumerator SpawnNow(int i)
    {
        int enemiesToSpawn = Random.Range(minEnemiesToSpawn, maxEnemiesToSpawn);        
        while (spawning == true)
        {            
            while (enemiesToSpawn > 0)
            {                
                enemiesToSpawn--;
                Instantiate(enemy, spawners[i].transform.position, Quaternion.identity);
                yield return new WaitForSeconds(0.5f);
            }
            spawning = false;
        }
        
    }
}