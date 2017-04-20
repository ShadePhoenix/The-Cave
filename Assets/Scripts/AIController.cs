using UnityEngine;
using System.Collections;

public class AIController : MonoBehaviour 
{
    // [Tooltip("Add the player as a target for nav mesh.")]
    //public Transform player;
    [Tooltip("The range that it can target the players base from. There should always be some object in range or they will wander aimlessly.")]
    public float targetRange;
    [Tooltip("Health of the enemy.")]
    public int health = 10;
    [Tooltip("How much damage you take from normal turret bullets.")]
    public int normalTurretDamageTaken = 1;
    [Tooltip("How much damage you take from the big player turret bullets.")]
    public int bigTurretDamageTaken = 5;
    //public enum Type
    //{
    //    Normal,
    //    Fast,
    //    Tank
    //};

    //[Tooltip("The type of enemy you choose.")]
    //public Type type;

    private GameObject hero;   
    private GameObject target;
    private UnityEngine.AI.NavMeshAgent m_agent;
    public LayerMask mask;
   
    private Collider[] structuresInRange;
    private float currentTargetDis = Mathf.Infinity;

    private GameObject[] structures;

    private bool targetStructureSet = false;
    private bool playerTargetSet = false;

    void Start () 
	{
		m_agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        hero = GameObject.FindGameObjectWithTag("Player");        
        structures = GameObject.FindGameObjectsWithTag("Base");       
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

        if(health <= 0)
        {
            gameObject.SetActive(false);
        }        
	}


    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Bullet")
        {
            other.gameObject.SetActive(false);
            health -= normalTurretDamageTaken;            
        }
    }   
}
