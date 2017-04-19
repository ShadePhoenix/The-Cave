using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerSpawner : MonoBehaviour {
    private GameController gameController;
	// Use this for initialization
	void Start () {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update () {
		
	}
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Entrance")
        {
            gameObject.GetComponent<PlayerController>().enabled = true;
            gameObject.GetComponent<PlayerController>().breath = gameObject.GetComponent<PlayerController>().maxBreath;
            gameObject.GetComponent<PlayerController>().boost = 100;
            gameController.SpawnMenu(false);
            gameObject.GetComponent<PlayerSpawner>().enabled = false;
        }

    }
    public void NextEntrance(int direction)
    {
        if (gameController.inMenu == false)
        {
            gameController.WhichEntrance();
            var target = (gameController.currentEntrance + direction) % gameController.entrances.Length;
            if ((gameController.currentEntrance + direction) < 0)
            {
                target = gameController.entrances.Length - 1;
            }
            gameObject.transform.position = gameController.entrances[target].transform.position;
        }
    }
    public void Dive()
    {
        gameObject.GetComponent<NavMeshAgent>().enabled = true;
        gameObject.GetComponent<NavMeshAgent>().destination = new Vector3(transform.position.x, transform.position.y, transform.position.z-10);
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>().player = gameObject;
        gameObject.layer = 0;
    }
}
