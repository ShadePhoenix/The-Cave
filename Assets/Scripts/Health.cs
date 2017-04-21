using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {   

    [Tooltip("Health of the character.")]
    public int startHealth = 10;
    //[HideInInspector]
    public float currentHealth;

    // Use this for initialization
    void Start () {
        currentHealth = startHealth;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
