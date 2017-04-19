using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {
    private GameObject player;
    private NavMeshAgent agent;
    public float rotationSpeed;
    public float homingDistance;
    public float visionArc = 180;
    public float visionDistance = 20;
    public float giveUp=5;
    public float agentSpeed;
    public float agentAccel =8;
    public bool seePlayer;
    public GameObject wayPointHeart;
    private float huntingTimer;
    private Vector3 target;
    private bool hunting;
    private Quaternion lookRotation;
    private Vector3 direction;
    private RaycastHit hit;
    private Transform[] waypoints;
    private int destPoint = 0;
    private GameController gameController;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        hunting = false;
        seePlayer = false;
        waypoints = wayPointHeart.GetComponentsInChildren<Transform>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        /* foreach (Transform i in waypoints)
         {
             if (i == wayPointHeart.transform)
             {

             }

         }*/
    }

    // Update is called once per frame
    void Update () {
        Vector3 visionCheck = player.transform.position - transform.position;
        float angle = Vector3.Angle(visionCheck, transform.forward);
       // print(angle);
        if (angle<visionArc)
        {
            if (Physics.Raycast(transform.position,player.transform.position - transform.position,out hit, visionDistance))
            {
                if (hit.transform.tag == "Player" || hit.transform.tag == "PlayerBody")
                {
                    seePlayer = true;
                }
                else seePlayer = false; 

            }
            else seePlayer = false;
        }
        else seePlayer = false;
        if (seePlayer && !hunting)
        {
            hunting = true;
            gameObject.GetComponent<AudioSource>().Play();
            agent.speed = 2 * agentSpeed;
            agent.acceleration = agentAccel * 2;
        }

        if (hunting)
        {

            if (!seePlayer)
            {
                huntingTimer += Time.deltaTime;
            }
            if (seePlayer)
            {
                huntingTimer = 0;
            }
            if (huntingTimer > giveUp)
            {
                EndHunt();
            }
            if (Vector3.Distance(player.transform.position, transform.position) > homingDistance && seePlayer)
            {
                target = player.GetComponent<PlayerController>().enemyTarget;
            }
            if (Vector3.Distance(player.transform.position, transform.position) < homingDistance && seePlayer)
            {
                target = player.transform.position;
            }
            //target = target * Mathf.Clamp(((Vector3.Distance(player.transform.position , transform.position))/10),0,1);
            //target = player.GetComponent<PlayerController>().enemyTarget* Mathf.InverseLerp(0,1,Vector3.Distance(player.transform.position,transform.position));
            agent.destination = target;
            direction = (target - transform.position).normalized;
            lookRotation = Quaternion.LookRotation(direction);
            //transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed); // makes the seal significantly more agile & sticky
        }
        if (!hunting & agent.remainingDistance< 0.5f)
        {
            GoToNextPoint();
        }
	}
    public void EndHunt()
    {
        agent.speed = agentSpeed;
        agent.acceleration = agentAccel;
        if (hunting)
        {
            GoToNextPoint();
        }
        hunting = false;
    }

    void GoToNextPoint()
    {
        if (waypoints.Length == 0)
        {
            return;
        }
        agent.destination = waypoints[destPoint].position;
        destPoint = (destPoint + 1) % waypoints.Length;
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag=="PlayerBody")
        {
            
                Rigidbody rb = player.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                rb.useGravity = true;
                player.GetComponent<NavMeshAgent>().enabled = false;
                rb.AddForce(gameObject.transform.forward * 1000);
                foreach (Transform child in player.transform)
                {
                    child.transform.parent = null;
                }
                player.transform.parent = null;
                player.GetComponent<PlayerController>().enabled = false;
                gameController.GameOver("You got eaten");
            

        }
       // Debug.Log("collision");
    }
}
