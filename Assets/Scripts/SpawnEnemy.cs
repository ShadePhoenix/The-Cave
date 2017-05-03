using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [Tooltip("Add the enemy to be spawned.")]
    public Transform enemy;   
    [Tooltip("Max enemies that can spawn at a time from the start. This increases by 1 every night.")]
    public int maxEnemiesToSpawn = 3;
    [Tooltip("Min enemies that can spawn at a time from the start.")]
    public int minEnemiesToSpawn = 1;
    //[Tooltip("How slowly enemies spawn (in seconds) at the start of the game.")]
    //public float startSpawnInterval = 3;
    //[Tooltip("The shortest interval between enemy spawns. Enemies will never spawn more frequently than this.")]
    //public float finalSpawnInterval = 1;
    //[Tooltip("This amount of seconds is taken off the frequency between spawns every night.")]
    //public float spawnFrequencyIncrease = 0.1f;
    [Tooltip("How slowly enemies spawn (in seconds) at the start of the game.")]
    public float spawnInterval = 3;

    //private List<Transform> enemies = new List<Transform>(); // pool of enemies
    //private int enemiesToSpawn = 1000;
    //private bool genEnemies = false;

    //private float timeUntilSpawn = 0;
    private GameObject[] spawners;   
    private bool spawning = false;
    private bool nightTime;
    
    void Start()
    {        
        spawners = GameObject.FindGameObjectsWithTag("Spawner");       
        nightTime = DayNight.isNight;
        StartCoroutine("Spawn");
    }

    void Update()
    {
        if (nightTime != DayNight.isNight)
        {
            if (DayNight.isNight)
            {
                StartCoroutine("Spawn");
            }
        }
        nightTime = DayNight.isNight;
    } 

    private IEnumerator Spawn()
    {
        if (spawners.Length > 0)
        {
            while (DayNight.isNight)
            {
                spawning = true;
                int rSpawn = Random.Range(0, spawners.Length);
                StartCoroutine("SpawnNow", rSpawn);
                yield return new WaitForSeconds(spawnInterval);
            }
        }      
    }
   
    private IEnumerator SpawnNow(int i)
    {        
            int enemiesToSpawn = Random.Range(minEnemiesToSpawn + ((int)DayNight.nightNumber / 2), maxEnemiesToSpawn + ((int)DayNight.nightNumber) + 2);
            while (spawning == true)
            {
                while (enemiesToSpawn > 0)
                {
                    enemiesToSpawn--;
                    Instantiate(enemy, spawners[i].transform.position, Quaternion.identity);
                    yield return new WaitForSeconds(1.5f);
                }
                spawning = false;
            }                    
    }


}