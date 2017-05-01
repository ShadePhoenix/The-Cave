using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOrTarget : MonoBehaviour {

    public enum TargetType
    {
        Player,
        Battlement,
        Turret,
        Castle,
        Non_Target
    };

    public TargetType targetType;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
