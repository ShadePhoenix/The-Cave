using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour {

    public GameObject followObject;

    public Vector3 offset;

    // Use this for initialization
    void Start()
    {
        followObject = GameObject.FindGameObjectWithTag("Player");
        transform.position = followObject.transform.position + offset;
        transform.LookAt(followObject.transform.position);
        //offset = new Vector3(0, 20, 20);
    }
	// Update is called once per frame
	void Update ()
    {
        transform.position = Vector3.Lerp(transform.position, followObject.transform.position + offset, Time.deltaTime * 5);
    }
}
