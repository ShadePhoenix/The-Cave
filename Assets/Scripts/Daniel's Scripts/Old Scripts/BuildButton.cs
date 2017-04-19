using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildButton : MonoBehaviour {

    public GameObject turretPrefab;

    //Image turretIcon;

    Button button;
    Text buttonText;

    TurretAI turretScript;

	// Use this for initialization
	void Start ()
    {
        turretScript = turretPrefab.GetComponent<TurretAI>();
        buttonText = GetComponentInChildren<Text>();
        button = GetComponent<Button>();
        //turretIcon = turretScript.turretIcon;
        //button.image = turretIcon;
        buttonText.text = turretScript.turretName + "\n $" + turretScript.cost;
        button.onClick.AddListener(Build);
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void Build()
    {
        GetComponentInParent<UIManager>().BuildButton(turretPrefab);
    }
}