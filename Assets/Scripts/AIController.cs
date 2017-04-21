using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AIController : MonoBehaviour 
{    
    [Tooltip("The range that it can target the players base from. There should always be some object in range or they will wander aimlessly.")]
    public float targetRange;
    [Tooltip("Health of the enemy.")]
    public int startHealth = 10;
    private float currentHealth;
    [Tooltip("How much damage you take from normal turret bullets.")]
    public int normalTurretDamageTaken = 1;
    [Tooltip("How much damage you take from the big player turret bullets.")]
    public int bigTurretDamageTaken = 5;
    [Tooltip("How much damage you deal the player.")]
    public int damageDealt = 1;

    private Animator anim;

    private GameObject hero;
    private playerController plController; 
    private GameObject target;
    private UnityEngine.AI.NavMeshAgent m_agent;
   
    private Collider[] structuresInRange;
    private float currentTargetDis = Mathf.Infinity;
    private float speed = 0f;

    private GameObject[] structures;

    private bool targetStructureSet = false;
    private bool playerTargetSet = false;
    private bool attacking = false;

    private float animationTimeLeft = 1;
    private Vector3 lastPos;

    public Image healthBarFill;
    public GameObject healthBar;

    void Start () 
	{
        currentHealth = startHealth;
		m_agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        hero = GameObject.FindGameObjectWithTag("Player");
        plController = hero.GetComponent<playerController>();
        structures = GameObject.FindGameObjectsWithTag("Base");
        anim = GetComponent<Animator>();
    }		
	void Update () 
	{
        HealthUpdate();
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
            Destroy(gameObject);
        }
        
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            animationTimeLeft -= Time.deltaTime;
            float dist = Vector3.Distance(transform.position, hero.transform.position);
            if (animationTimeLeft <= 0 && (dist <= 1 && dist >= -1))
            {
                animationTimeLeft = 1;
                plController.health -= damageDealt;
            }
        }
        if (lastPos != transform.position)
        {
            speed = Mathf.Clamp01(speed+Time.deltaTime);
            anim.SetFloat("Blend", speed);
        }
        else
        {
            speed = Mathf.Clamp01(speed - Time.deltaTime);
            anim.SetFloat("Blend",speed);
        }
    }

    void OnCollisionStay(Collision other)
    {
        if(other.gameObject.tag == "Player" )
        {
            anim.SetTrigger("Attack");           
        }
    }        

    void OnTriggerEnter(Collider other)
    {        
        if (other.gameObject.tag == "Bullet")
        {
            Destroy(other);
            currentHealth -= normalTurretDamageTaken;            
        }
        if (other.gameObject.tag == "Player Bullet")
        {
            Destroy(other);
            currentHealth -= bigTurretDamageTaken;            
        }
    }

    void HealthUpdate()
    {
        healthBar.transform.position = transform.position + new Vector3(0, 1, 1);
        healthBar.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
        healthBarFill.fillAmount = currentHealth / startHealth;
        if (currentHealth <= 0)
        {
            //Destroy enemy and play particle effects/animation
            //GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIManager>().GameOver();
        }
    }
}
