using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceUI : MonoBehaviour {
    public GameObject energy;
    public GameObject constructionMaterials;
    public GameObject gold;
    private Text energyText;
    private Text constructionText;
    private Text goldText;

    // Use this for initialization
    void Start () {
        energyText = energy.GetComponent<Text>();
        constructionText = constructionMaterials.GetComponent<Text>();
        goldText = gold.GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        energyText.text = "" + UIManager.energy;
        constructionText.text = "" + UIManager.conMat;
        goldText.text = "" + UIManager.gold;
    }
}
