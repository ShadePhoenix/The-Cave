using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour {

    [Tooltip("Add the enemy to be spawned.")]
    public Transform enemy;
    [Tooltip("The time (in seconds) between enemies spawning.")]
    public float enemySpawnInterval = 3;
	[Tooltip("The maximum number of enemies that can spawn on this level.")]
	public int maxEnemiesToSpawn;
	[Tooltip("The minimum number of enemies that can spawn on this level.")]
	public int minEnemiesToSpawn;
    
	private float timeUntilSpawn = 0;
    private GameObject[] spawners;
    //private bool addedSpawners = false;
	private bool spawning = false;
	

    // Use this for initialization
    void Start()
    {
       // GameObject[] tempSpawners;
        spawners = GameObject.FindGameObjectsWithTag("Spawners");
    }

    // Update is called once per frame
    void Update()
    {
		if (spawning == false)
		{
			timeUntilSpawn -= Time.deltaTime;
			
			if (timeUntilSpawn <= 0)
			{
				spawning = true;				
				StartCoroutine("SpawnEnemy");
				timeUntilSpawn = enemySpawnInterval;
			}
		}       
    }

   private IEnumerator SpawnEnemy()
    {
		private int randSpawn = Random.Range(0, spawners.length); // pick a random spawner index in the array
		private int enemiesToSpawn = Random.Range(minEnemiesToSpawn, maxEnemiesToSpawn);
        while (enemiesToSpawn > 0)
        {
			enemiesToSpawn--;
			Instantiate(enemy, spawners[randSpawn].transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.5);            
        }
    }
