using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour {
    public GameObject fish;
    public int maxFish;
    public float spawnTimer;
    private float cubeX;
    private float cubeZ;
    private float timer = 0;
    private float randX;
    private float randZ;
    private BoxCollider box;
	// Use this for initialization
	void Start () {
        box = gameObject.transform.GetComponentInChildren<BoxCollider>();
        cubeX = box.transform.localScale.x / 2.0f;
        cubeZ = box.transform.localScale.z / 2.0f;
        SpawnFish(maxFish);
        maxFish += 1;//because the parent has 1 child at start
    }
	
	// Update is called once per frame
	void Update () {
		if (timer < spawnTimer)
        {
            timer += Time.deltaTime;
        }
        if (spawnTimer <= timer)
        {
            if (maxFish > transform.parent.childCount)
            {
                SpawnFish(1);
            }
        }
	}
    void SpawnFish(int runs)
    {
        for (int i = 0; i < runs; i++)
        {
            randX = Random.Range(-cubeX, cubeX);
            randZ = Random.Range(-cubeZ, cubeZ);
            GameObject babby = Instantiate(fish, new Vector3(randX+transform.position.x, transform.position.y, randZ+transform.position.z), transform.rotation);
            babby.transform.parent = transform.parent;
        }
        timer = 0;
    }
}
