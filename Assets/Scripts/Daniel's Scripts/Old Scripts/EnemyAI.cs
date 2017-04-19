using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]

public class EnemyAI : MonoBehaviour {


    [Tooltip("How much you get when you kill this unit")]
    public int moneyWorth;
    [Tooltip("How many points you get for killing this unit")]
    public int scoreWorth;
    [Tooltip("The amount of Health this unit has")]
    public float health = 10;
    [Tooltip("Damage dealt to player turret if it reaches it")]
    public int damageToPlayer = 10;
    [Tooltip("How fast the unit moves")]
    public float speed = 5;

    public Transform target;

    NavMeshAgent agent;

    float currentHealth;

    public GameObject healthBar;

    public Image healthBarFill;

    public float disLeft;

    GameObject canvas;

	// Use this for initialization
	void Start ()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(target.position);
        agent.speed = speed;
        currentHealth = health;
    }
	
	// Update is called once per frame
	void Update ()
    {
        calculatePathLength();
        HealthUpdate();
    }

    public float calculatePathLength()
    {
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path);
        if (path.corners.Length < 2)
        {
            return 0;
        }
        Vector3 previousCorner = path.corners[0];
        float lengthSoFar = .0f;
        for (int i = 1; i < path.corners.Length; i++)
        {
            Vector3 currentCorner = path.corners[i];
            lengthSoFar += Vector3.Distance(previousCorner, currentCorner);
            previousCorner = currentCorner;
        }
        disLeft = lengthSoFar;
        return lengthSoFar;
    }

    public void TakeDamage(int damage)
    {
        currentHealth = currentHealth - damage;
        if (currentHealth <= 0)
        {
            UIManager.score += scoreWorth;
            UIManager.money += moneyWorth;
            canvas.GetComponent<UIManager>().UpdateStats();
//            EnemySpawner.defeatedEnemies++;
            Destroy(gameObject);
        }
    }

    void HealthUpdate()
    {
        healthBar.transform.position = transform.position + new Vector3(0, 1, 2);
        healthBar.transform.rotation = Quaternion.Euler(new Vector3(90,0,0));
        healthBarFill.fillAmount = currentHealth / health;
    }
}