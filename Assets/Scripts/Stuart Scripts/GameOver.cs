﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {

    //private GameObject gc;
    //private AITargets aiTargets;
    //private BuildNode[] nodes;
    //private GameObject[] nodesObj;
    //private Health[] nodesHealth;
    //private playerController player;
    //private Health playerHealth;
    //private CastleController castle;
    //private Health castleHealth;
    //public Transform playerArea;
    //private GameObject energyPool;
    //private GameObject conMine;

    public Button yourButton;

    // Use this for initialization
    void Start ()
    {
        Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);

        //player = FindObjectOfType<playerController>();
        //playerHealth = player.GetComponent<Health>();
        //castle = FindObjectOfType<CastleController>();
        //castleHealth = castle.GetComponent<Health>();
        //nodes = FindObjectsOfType<BuildNode>();
        //nodesObj = new GameObject[nodes.Length];
        //nodesHealth = new Health[nodes.Length];
        //energyPool = GameObject.Find("EnergyPool");
        //conMine = GameObject.Find("Mine");

        //for (int i = 0; i < nodes.Length; i++)
        //{
        //    nodesObj[i] = nodes[i].gameObject;
        //    nodesHealth[i] = nodesObj[i].gameObject.GetComponent<Health>();   
        //}

        //gc = GameObject.FindObjectOfType<Main>().gameObject;
        //aiTargets = gc.GetComponent<AITargets>();
    }

    void TaskOnClick()
    {
        RestartGame();
        gameObject.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("GameLevelAttack");
        //AIController[] enemies = FindObjectsOfType<AIController>();
        //GameObject[] enemiesObj = new GameObject[enemies.Length];
        //for(int i = 0; i < enemies.Length; i++)
        //{
        //    enemiesObj[i] = enemies[i].gameObject;
        //    Destroy(enemiesObj[i]);
        //}

        //for (int i = 0; i < nodes.Length; i++)
        //{
        //    if (nodes[i].healthBar != null)
        //        nodesHealth[i].currentHealth = 0;
        //    else
        //        nodes[i].ResetBuildNode();
        //}
        //GameObject[] coins = GameObject.FindGameObjectsWithTag("Gold");
        //for (int i = 0; i < coins.Length; i++)
        //{
        //    //enemiesObj[i] = enemies[i].gameObject;
        //    Destroy(coins[i]);
        //}

        //UIManager.gold = 0;
        //UIManager.energy = UIManager.s_startEnergy;
        //UIManager.conMat = UIManager.s_startingScrap;
        //conMine.GetComponent<ResourceNode>().currentResources = 0;
        //energyPool.GetComponent<ResourceNode>().currentResources = 0;

        //DayNight timeDay = FindObjectOfType<DayNight>();
        //timeDay.dayRotate = 0;
        //timeDay.nightTimer = 0;
        //DayNight.nightNumber = 0;

        //playerHealth.currentHealth = playerHealth.startHealth;
        //castleHealth.currentHealth = castleHealth.startHealth;
        //player.transform.position = playerArea.transform.position;

        //aiTargets.Init();

        //Time.timeScale = 1;        
    }
}
