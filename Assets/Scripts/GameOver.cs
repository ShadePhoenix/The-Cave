using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour {

    private GameObject gc;
    private AITargets aiTargets;

    // Use this for initialization
    void Start ()
    {
        gc = GameObject.FindObjectOfType<Main>().gameObject;
        aiTargets = gc.GetComponent<AITargets>();
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void RestartGame()
    {
        aiTargets.Init();
    }
}
