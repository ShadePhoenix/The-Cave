using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour {

    public GameObject followObject;
    public Vector3 playerOffset;
    public Vector3 castleOffset;
    [HideInInspector]
    public bool followingPlayer = true;

    // Use this for initialization
    void Start()
    {
        followObject = GameObject.FindGameObjectWithTag("Player");
        transform.position = followObject.transform.position + playerOffset;
        //transform.LookAt(followObject.transform.position);
        transform.eulerAngles = new Vector3(40, 0, 0);
        //offset = new Vector3(0, 20, 20);
    }
	// Update is called once per frame
	void Update ()
    {
        if(followingPlayer)
        {
            transform.position = followObject.transform.position + playerOffset;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, followObject.transform.position + castleOffset, Time.deltaTime * 5);
        }
        
    }

    
}
