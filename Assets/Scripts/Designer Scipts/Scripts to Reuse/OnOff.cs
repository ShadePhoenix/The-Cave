using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOff : MonoBehaviour {
    //private bool IsOn;
	// Use this for initialization
	void Start () {
        gameObject.SetActive(false);
       // IsOn = false;
	}
	
	// Update is called once per frame
	public void OnOrOff()
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
           // IsOn = true;
        }
        else {
            gameObject.SetActive(false);
         //   IsOn = false;
            }
    }
}
