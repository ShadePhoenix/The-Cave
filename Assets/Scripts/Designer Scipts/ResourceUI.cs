﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceUI : MonoBehaviour {
    public GameObject energy;
    public GameObject constructionMaterials;
    private Text energyText;
    private Text constructionText;
	// Use this for initialization
	void Start () {
        energyText = energy.GetComponent<Text>();
        constructionText = constructionMaterials.GetComponent<Text>();

	}
	
	// Update is called once per frame
	void Update () {
        energyText.text = "" + UIManager.energy;
        constructionText.text = "" + UIManager.conMat;
    }
}