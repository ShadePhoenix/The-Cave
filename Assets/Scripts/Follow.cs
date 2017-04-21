using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour {

    public GameObject followObject;

    public Vector3 offset;

    // Use this for initialization
    void Start()
    {
        //offset = new Vector3(0, 20, 20);
    }
	// Update is called once per frame
	void Update ()
    {
        transform.position = followObject.transform.position + offset;        
    }
}
