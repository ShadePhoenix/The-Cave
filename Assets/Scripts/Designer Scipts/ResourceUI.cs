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

    public Text highScoreText;


    // Use this for initialization
    void Start ()
    {
        if (!PlayerPrefs.HasKey("highScore"))
            InitializeHS();
        else
            highScoreText.text = PlayerPrefs.GetInt("highScore").ToString();
        energyText = energy.GetComponent<Text>();
        constructionText = constructionMaterials.GetComponent<Text>();
        goldText = gold.GetComponent<Text>();
        UpdateHS();
    }
	
	// Update is called once per frame
	void Update () {
        energyText.text = "" + UIManager.energy;
        constructionText.text = "" + UIManager.conMat;
        goldText.text = "" + UIManager.gold;
    }

    void InitializeHS()
    {
        PlayerPrefs.SetInt("highScore", 0);
        PlayerPrefs.Save();
    }

    void UpdateHS()
    {
        if (UIManager.gold > PlayerPrefs.GetInt("highScore"))
        {
            PlayerPrefs.SetInt("highScore", UIManager.gold);
            PlayerPrefs.Save();
        }
    }
}
