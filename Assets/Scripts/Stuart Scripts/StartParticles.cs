using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartParticles : MonoBehaviour {

    private ParticleSystem thisSystem;

    // Use this for initialization
    private void Start()
    {
        thisSystem = gameObject.GetComponent<ParticleSystem>();
        thisSystem.Play();
    }

    void Awake ()
    {
        
        //thisSystem.Play();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
