using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [Tooltip("Add the enemy to be spawned.")]
    public GameObject enemy;
    [Tooltip("The time (in seconds) between enemies spawning.")]
    public float enemySpawnInterval = 3;
    [Tooltip("Max enemies that can spawn at a time from the start. This increases by 1 every night.")]
    public int maxEnemiesToSpawn = 3;
    [Tooltip("Min enemies that can spawn at a time from the start.")]
    public int minEnemiesToSpawn = 1;
    [Tooltip("How slowly enemies spawn at the start of the game. Gradually decreses.")]
    public float startSpawnTimer = 1;
    [Tooltip("The shortest interval between enemy spawns. Enemies will never spawn more frequently than this.")]
    public float finalSpawnTimer = 1;

    private List<GameObject> enemies = new List<GameObject>(); // pool of enemies
    private int enemiesToSpawn = 1000;
    private bool genEnemies = false;

    private float timeUntilSpawn = 0;
    private GameObject[] spawners;   
    private bool spawning = false;
    
    void Start()
    {        
        spawners = GameObject.FindGameObjectsWithTag("Spawner");
        StartCoroutine("Spawn");
    }

    void Update()
    {
        //GenEnemies();
        //if (genEnemies)
        //{
        //    StartCoroutine("Spawn");
        //}
        
    }

    void GenEnemies()
    {
        if(genEnemies == false)
        {
            //genEnemies = true;
            //for (int i = 0; i < enemiesToSpawn; i++)
            //{
            //    enemies.Add((GameObject)Instantiate(enemy, spawners[0].transform.position, Quaternion.identity));
            //    enemies[i].gameObject.SetActive(false);
            //}
        }
    }

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
        int enemiesToSpawn = Random.Range(minEnemiesToSpawn, maxEnemiesToSpawn + (int)DayNight.nightNumber);        
        while (spawning == true)
        {            
            while (enemiesToSpawn > 0)
            {                
                enemiesToSpawn--;
                //for (int x = 0; x < enemiesToSpawn; x++)
                //{
                //    if (enemies[x].gameObject.activeSelf == false)
                //    {
                //        enemies[x].gameObject.SetActive(true);
                //        enemies[x].transform.position = spawners[x].transform.position;
                //        break;
                //    }
                //}                     
                Instantiate(enemy, spawners[0].transform.position, Quaternion.identity);
                yield return new WaitForSeconds(0.5f);
            }
            spawning = false;
        }        
    }


}