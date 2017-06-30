using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour {

    public Material resourceReady;
    public Material resourceEmpty;
    public int resourceValue;
    public float respawnTime = 60;

    private Health health;
    private bool mined = false;    
    private float timeToRespawn;
    private Light light;

    public enum ResourceType
    {
        Cyrstal,
        Ore
    };

    public ResourceType type = ResourceType.Cyrstal;

	// Use this for initialization
	void Start ()
    {
        health = GetComponent<Health>();
        timeToRespawn = respawnTime;
        light = GetComponent<Light>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        CheckHealth();

        if(mined)
        {
            CheckRespawn();
        }
	}

    void CheckHealth()
    {
        //print("Checking health");
        if (health.currentHealth <= 0 /*&& !mined*/)
        {
            if(!mined && type == ResourceType.Ore)
            {
                UIManager.conMat += resourceValue;
                light.enabled = false;
            }
                
            else if (!mined && type == ResourceType.Cyrstal)
            {
                UIManager.energy += resourceValue;
                light.enabled = false;
            }                

            GetComponent<MeshRenderer>().material = resourceEmpty;
            mined = true;
           
        }            
        else /*if (health.currentHealth > 0 && mined)*/
        {
            GetComponent<MeshRenderer>().material = resourceReady;
            mined = false;            
        }            
    }

    void CheckRespawn()
    {
        timeToRespawn -= Time.deltaTime;

        if(timeToRespawn <= 0)
        {
            mined = false;
            health.currentHealth = health.startHealth;
            timeToRespawn = respawnTime;
            light.enabled = true;
        }
    }
}
