using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gold : MonoBehaviour {

    public GameObject gold;
    private Text goldText;

    // Use this for initialization
    void Start () {
        goldText = gold.GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        goldText.text = "" + UIManager.energy;
    }
}
