using UnityEngine;
using System.Collections;

public class AIController : MonoBehaviour 
{    
    [Tooltip("The range that it can target the players base from. There should always be some object in range or they will wander aimlessly.")]
    public float targetRange;
    [Tooltip("Health of the enemy.")]
    public int startHealth = 10;
    private int currentHealth;
    [Tooltip("How much damage you take from normal turret bullets.")]
    public int normalTurretDamageTaken = 1;
    [Tooltip("How much damage you take from the big player turret bullets.")]
    public int bigTurretDamageTaken = 5;

    private Animator anim;

    private GameObject hero;   
    private GameObject target;
    private UnityEngine.AI.NavMeshAgent m_agent;
   
    private Collider[] structuresInRange;
    private float currentTargetDis = Mathf.Infinity;

    private GameObject[] structures;

    private bool targetStructureSet = false;
    private bool playerTargetSet = false;
    private bool attacking = false;

    void Start () 
	{
        currentHealth = startHealth;
		m_agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        hero = GameObject.FindGameObjectWithTag("Player");        
        structures = GameObject.FindGameObjectsWithTag("Base");
        anim = GetComponent<Animator>();
    }		
	void Update () 
	{          
        if (playerController.playerAtBase == false)
        {            
            targetStructureSet = false;
            m_agent.SetDestination(hero.transform.position);
        }
        else
        {
            if(targetStructureSet == false)
            {
                targetStructureSet = true;
                int rStruct = Random.Range(0, structures.Length);
                m_agent.SetDestination(structures[rStruct].transform.position);
            }            
        }

        if(currentHealth <= 0)
        {
            gameObject.SetActive(false);
        }

        if(attacking)
        {
            anim.SetTrigger("Attack");
            attacking = false;
        }       
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Player" )
        {
            attacking = true;
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "Player")
            attacking = false;
    }

    void OnTriggerEnter(Collider other)
    {        
        if (other.gameObject.tag == "Bullet")
        {
            other.gameObject.SetActive(false);
            currentHealth -= normalTurretDamageTaken;            
        }
        if (other.gameObject.tag == "Player Bullet")
        {            
            other.gameObject.SetActive(false);
            currentHealth -= bigTurretDamageTaken;            
        }
    }  
}
