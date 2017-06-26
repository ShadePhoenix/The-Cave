using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour {

    public int resourceValue;
    private Health health;
    private bool destroyed = false;
    public float respawnTime = 60;
    private float timeToRespawn;

    public enum ResourceType
    {
        Cyrstal,
        Ore
    };

    public ResourceType type = ResourceType.Cyrstal;

	// Use this for initialization
	void Start () {
        health = GetComponent<Health>();
        timeToRespawn = respawnTime;

    }
	
	// Update is called once per frame
	void Update () {
        CheckHealth();

        if(destroyed)
        {
            CheckRespawn();
        }
	}

    void CheckHealth()
    {
        //print("Checking health");
        if (health.currentHealth <= 0)
        {
            this.gameObject.SetActive(false);
            destroyed = true;
        }            
        else
            this.gameObject.SetActive(true);
    }

    void CheckRespawn()
    {
        timeToRespawn -= Time.deltaTime;

        if(timeToRespawn <= 0)
        {
            destroyed = false;
            health.currentHealth = health.startHealth;
        }
    }

    void Destroyed()
    {        

    }
}
