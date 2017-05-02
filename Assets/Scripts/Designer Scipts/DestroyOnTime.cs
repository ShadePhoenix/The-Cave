using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTime : MonoBehaviour {
    public float lifeSpan = 120.0f;
    private float timer ;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (lifeSpan < timer)
        {
            Destroy(gameObject);
        }
	}
}
