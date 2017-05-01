using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Vector3 target;
    private NavMeshAgent agent;
    private float speed;
    public float boost;
    public float breath;
    public bool boosting = false;
    public float breathRate = 3.3f;
    public float exhaustionRate;
    public float regenRate;
    public float maxBreath;
    public GameObject[] enemies;
    public Vector3 enemyTarget;
    private Vector3 lastPosition;
    private float currentSpeed;
    private Slider boostSlider;
    private Slider breathtSlider;
    private GameController gameController;
    //private ParticleSystem particles;
    private float angularSpeed;
    //add breath decreaseing while boosting
    //public float speedDecay;
    // Use this for initialization
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        agent = GetComponent<NavMeshAgent>();
        speed = agent.speed;
        angularSpeed = agent.angularSpeed;
        lastPosition = transform.position;
        boostSlider = GameObject.Find("BoostSlider").GetComponent<Slider>();
        breathtSlider = GameObject.Find("BreathSlider").GetComponent<Slider>();
        boostSlider.maxValue = boost;
        breathtSlider.maxValue = breath;
        boostSlider.value = boost;
        breathtSlider.value = breath;
        maxBreath = breath;
        //particles = GetComponentInChildren<ParticleSystem>();
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemyTarget = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        breath -= breathRate * Time.deltaTime;
        if (boost < 100 && !boosting)
        {
            boost += regenRate * Time.deltaTime;
        }
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
        {
            if (Vector3.Distance(transform.position, hit.point) > 3 || boosting)
                agent.destination = hit.point;
            else agent.destination = transform.position;
        }
        if (Input.GetMouseButtonDown(0))
        {
            agent.speed = speed * 2;
            agent.acceleration = speed * 2;
            agent.angularSpeed *= 2;
            boosting = true;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            gameController.score += 1000;
        }
        if (boosting)
        {
            boost -= exhaustionRate * Time.deltaTime;
            breath -= exhaustionRate * Time.deltaTime;
        }
        if ((Input.GetMouseButtonUp(0) && agent.speed > speed) || boost < 0)
        {
            agent.speed = speed;
            agent.acceleration = speed;
            agent.angularSpeed = angularSpeed;
            boosting = false;
        }
        if (breath < 0)
        {
            gameController.GameOver("You Drowned");
            agent.enabled = (false);
        }
        currentSpeed = ((transform.position - lastPosition).magnitude) / Time.deltaTime;
        if (!gameController.inMenu)
        {
            enemyTarget = (transform.position + transform.forward * currentSpeed);// target for enemy slightly ahead of player, increases accuracy. Also used by camera to give movement
        }
        lastPosition = transform.position;
        breathtSlider.value = breath;
        boostSlider.value = boost;
        //particles.startSpeed = currentSpeed / 2;
        //particles.emissionRate = Mathf.Clamp((currentSpeed/2),2,20);
        //particles.emissionRate = currentSpeed / 2;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PickUp")
        {
            if (other is BoxCollider)
            {
                Destroy(other.transform.parent.gameObject);
                gameController.score += other.GetComponent<FishScript>().foodValue;
            }
            else if (other is SphereCollider)
            {
                other.GetComponent<FishScript>().Flee();
            }
        }
        if (other.tag == "Entrance")
        {
            gameObject.GetComponent<PlayerSpawner>().enabled = true;
            agent.enabled = false;
            //particles.emissionRate = 0;
            for (var i = 0; i < enemies.Length; i++)
            {
                enemies[i].SendMessage("EndHunt");
            }
            gameObject.layer = 2;
            gameObject.transform.position = other.transform.position;
            gameController.SpawnMenu(true);
            if (gameController.score >= gameController.oneStar)
            {
                GameObject.Find("EndLevel").GetComponent<Button>().interactable = true;

            }
            enemyTarget = transform.position;
            gameObject.GetComponent<PlayerController>().enabled = false;
        }
    }
}
