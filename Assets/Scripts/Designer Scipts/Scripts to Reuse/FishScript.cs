using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FishScript : MonoBehaviour {
    //private GameController gameController;
    private GameObject player;
    private Vector3 tether;
    private NavMeshAgent agent;
    public float range = 5;
    public float fleeRefresh=5;
    private Vector3 destination;
   // private bool fleeing = false;
    private float timer;
    private float randomSize;
    
    public float fleeSpeed;
    public float baseSpeed;
    public float fleeDistance;
    public float fleeSpeedTime;
    public float fleeAngleRange;
    [Tooltip("Weight of food in grams")]
    public float foodValue;
    Vector3 startPosition;
    Quaternion startRotation;

    // Use this for initialization
    void Start () {
        //transform.parent = null;
        //gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        player = GameObject.FindGameObjectWithTag("Player");
        agent = transform.parent.GetComponent<NavMeshAgent>();
        NavMeshHit hit;//makes sure spawn point is valid
        NavMesh.SamplePosition(transform.position, out hit, range, NavMesh.AllAreas);
        transform.position = hit.position;
        tether = transform.position;
        destination = transform.position;
        randomSize = Random.Range(0.5f, 2.0f);
        transform.localScale *= randomSize;
        foodValue *= (randomSize * randomSize) ;
        timer = fleeRefresh;
        if (randomSize > 1)
        {
            baseSpeed *= randomSize;
            fleeSpeed *= randomSize;
            agent.speed = baseSpeed;
        }
        agent.radius *= randomSize;
        agent.acceleration = baseSpeed * 2;
        Wander();
	}
    void Wander()//makes fish wander around in an area around its spawn
    {
        //if (fleeing) fleeing = false;
        Vector3 randomPoint = Random.insideUnitSphere * range;
        randomPoint += tether;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomPoint, out hit, range, NavMesh.AllAreas);
        Vector3 finalPoint = hit.position;
        destination = finalPoint;
        agent.destination = destination;
        agent.speed = baseSpeed;
        agent.acceleration = baseSpeed * 2;
        //timer = fleeSpeedTime + 1;
    }
    // Update is called once per frame
    void Update () {
        timer += Time.deltaTime;
        if (Vector3.Distance (destination, transform.position)<randomSize+1)
        {
            Wander();
        }
        if (timer > fleeSpeedTime && agent.speed>baseSpeed)
        {
            agent.speed = baseSpeed;
            agent.acceleration = baseSpeed * 2;
        }
	}

    public void Flee() //causes fish to flee from player
    {

        Debug.Log("EEEE");
            if (timer > fleeRefresh)
            { 
            timer = 0;
            agent.speed = fleeSpeed;
            agent.acceleration = fleeSpeed * 2;
            }
            startPosition = gameObject.transform.position;
            startRotation = gameObject.transform.rotation;
            Vector3 rotate = new Vector3(0f, Random.Range(-fleeAngleRange, fleeAngleRange), 0f);
          //  Debug.Log(rotate);
            transform.rotation = Quaternion.LookRotation(transform.position - player.transform.position + rotate);
            destination = transform.position + (transform.forward * fleeDistance);
            NavMeshHit hit;
            NavMesh.SamplePosition(destination, out hit, fleeDistance, NavMesh.AllAreas);
            transform.position = startPosition;
            transform.rotation = startRotation;
            destination = hit.position;
            agent.SetDestination(hit.position);
        
    }

    /*void OnTriggerEnter (Collider other)
    {
        if (other.tag == "Player" && other.GetType()==typeof(BoxCollider))
        {
            Destroy(gameObject);
            gameController.score += 1; 
            Debug.Log("FEESH");
        }
    }*/
}
