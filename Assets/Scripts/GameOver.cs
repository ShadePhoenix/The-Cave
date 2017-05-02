using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour {

    private GameObject gc;
    private AITargets aiTargets;
    private BuildNode[] nodes;
    private GameObject[] nodesObj;
    private Health[] nodesHealth;
    private playerController player;
    private Health playerHealth;
    private CastleController castle;
    private Health castleHealth;

    // Use this for initialization
    void Start ()
    {
        player = FindObjectOfType<playerController>();
        playerHealth = player.GetComponent<Health>();
        castleHealth = castle.GetComponent<Health>();
        nodes = FindObjectsOfType<BuildNode>();
        nodesObj = new GameObject[nodes.Length];
        nodesHealth = new Health[nodes.Length];
        castle = FindObjectOfType<CastleController>();

        for (int i = 0; i < nodes.Length; i++)
        {
            nodesObj[i] = nodes[i].gameObject;
            nodesHealth[i] = nodesObj[i].GetComponent<Health>();
        }

        gc = GameObject.FindObjectOfType<Main>().gameObject;
        aiTargets = gc.GetComponent<AITargets>();
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void RestartGame()
    {
        AIController[] enemies = FindObjectsOfType<AIController>();
        GameObject[] enemiesObj = new GameObject[enemies.Length];
        for(int i = 0; i < enemies.Length; i++)
        {
            enemiesObj[i] = enemies[i].gameObject;
            Destroy(enemiesObj[i]);
        }

        for (int i = 0; i < nodes.Length; i++)
        {                        
            nodesHealth[i].currentHealth = 0;
        }

        UIManager.gold = 0;
        UIManager.energy = UIManager.s_startEnergy;
        UIManager.conMat = UIManager.s_startingScrap;

        DayNight timeDay = FindObjectOfType<DayNight>();
        timeDay.dayRotate = 0;
        timeDay.nightTimer = 0;
        DayNight.nightNumber = 0;

        playerHealth.currentHealth = playerHealth.startHealth;
        castleHealth.currentHealth = castleHealth.startHealth;

        aiTargets.Init();
    }
}
