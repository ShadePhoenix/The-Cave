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

    private int score;
    private int oldHS;

    // Use this for initialization
    void Start ()
    {
        //if (!PlayerPrefs.HasKey("highScore"))
        //    InitializeHS();
        //else
        //    highScoreText.text = PlayerPrefs.GetInt("highScore").ToString();

        if (PlayerPrefs.HasKey("highScore")  == false)
        {
            print("There is no high score");
            InitializeHS();
        }

        highScoreText.text = "Highscore: " + PlayerPrefs.GetInt("highScore", 0);        

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
        oldHS = PlayerPrefs.GetInt("highScore", 0);

        if (UIManager.gold > oldHS)
        {
            PlayerPrefs.SetInt("highScore", UIManager.gold);            
            highScoreText.text = "Highscore: " + PlayerPrefs.GetInt("highScore", 0);
        }
        PlayerPrefs.Save();
    }
}
