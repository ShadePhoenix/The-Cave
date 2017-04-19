using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildTurret : MonoBehaviour {

    public static GameObject targetBuildArea;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void Build(GameObject turretPrefab)
    {

        targetBuildArea = null;
    }
}
