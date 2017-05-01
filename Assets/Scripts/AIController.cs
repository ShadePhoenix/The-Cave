﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.AI;
using System.Collections.Generic;

[RequireComponent(typeof(AudioSource))]
public class AIController : MonoBehaviour 
{    
    [Tooltip("The range that it can target the players base from. There should always be some object in range or they will wander aimlessly.")]
    public float targetRange;
    //[Tooltip("Health of the enemy.")]
    //public int startHealth = 10;
    //private float currentHealth;

    [Tooltip("How much damage you take from normal turret bullets.")]
    public int normalTurretDamageTaken = 1;
    [Tooltip("How much damage you take from the big player turret bullets.")]
    public int bigTurretDamageTaken = 5;
    [Tooltip("How much damage you deal the player.")]
    public int damageDealt = 1;
    [Tooltip("The bullet to get damage from.")]
    public GameObject bullet;

    private BulletControl bulletScript;

    public GameObject deathEffect;
    public AudioClip[] sounds;
    private AudioSource audioPlayer;


    public enum EnemyType
    {
        Normal = 1,
        Fast = 2,
        Tank = 3,
        Boss = 4
    };

    [Tooltip("Set the enemy type for the prefab. This will determine random enemy spawns.")]
    public EnemyType type = EnemyType.Normal;

    private Animator anim;
    private GameObject hero;
    //private playerController plController; 
    private GameObject target;
    private PlayerOrTarget targType; // what kind of target is it
    private NavMeshAgent m_agent;
   
    private Collider[] structuresInRange;
    //private float currentTargetDis = Mathf.Infinity;

    private float animationSpeed = 0f;

    private List<GameObject> structures = new List<GameObject>();
    private List<PlayerOrTarget> strucTypes = new List<PlayerOrTarget>(); // the types of the structures, whether targetable or not
    
    private bool targetStructureSet = false;
    //private bool playerTargetSet = false;
    //private bool attacking = false;

    public float animationTimeLeft = 1;
    private Vector3 lastPos;

    public Image healthBarFill;
    public GameObject healthBar;

    private Health targetHealth;
    private Health myHealth;


    Transform marker;

    void Start () 
	{        
		m_agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        hero = GameObject.FindGameObjectWithTag("Player");
        //plController = hero.GetComponent<playerController>(); 
        anim = GetComponent<Animator>();
        myHealth = GetComponent<Health>();
        lastPos = transform.position;
        //marker = transform.FindChild("Marker");
        target = hero;
        audioPlayer = gameObject.GetComponent<AudioSource>();
        PopulateStructureLists();
    }		
	void Update () 
	{
        //marker.position = m_agent.destination;

        HealthUpdate();
        if (playerController.playerActive == true)
        {
            target = hero;
            targType = target.GetComponent<PlayerOrTarget>();
            targetStructureSet = false;
            m_agent.SetDestination(hero.transform.position);
        }
        else
        {
            if(targetStructureSet == false)
            {      
                targetStructureSet = true;
                int rStruct = Random.Range(0, structures.Count);
                target = structures[rStruct];
                targType = target.GetComponent<PlayerOrTarget>();

                Vector3 targetPos = structures[rStruct].transform.position;
                NavMeshHit myNavHit;
                if (targType.targetType == PlayerOrTarget.TargetType.Battlement && NavMesh.SamplePosition(targetPos, out myNavHit, 10, -1))
                {
                    targetPos = myNavHit.position;
                }
                else if (targType.targetType == PlayerOrTarget.TargetType.Castle && NavMesh.SamplePosition(targetPos, out myNavHit, 20, -1))
                {
                    targetPos = myNavHit.position;
                }

                m_agent.SetDestination(targetPos);
            }            
        }
        if(myHealth.currentHealth <= 0)
        {
            //spawns particle effect, and makes sure that the particle effect is its own object
            GameObject particles = Instantiate(deathEffect, transform.position, transform.rotation);
            particles.transform.parent = null;
            Destroy(gameObject);
        }
        
        // if the attack animation is playing, see if 1 second has passed and then do a distance check on the objects to be attacked
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {            
            animationTimeLeft -= Time.deltaTime;
            float dist = Vector3.Distance(transform.position, target.transform.position);            
            if ((animationTimeLeft <= 0 && targType.targetType == PlayerOrTarget.TargetType.Player) && (dist <= 1 && dist >= -1))
            {                
                animationTimeLeft = 1;
                targetHealth = target.GetComponent<Health>(); 
                targetHealth.currentHealth -= damageDealt;
            }
            else if ((animationTimeLeft <= 0 && targType.targetType == PlayerOrTarget.TargetType.Battlement ) && (dist <= 5 && dist >= 4.5))
            {
                animationTimeLeft = 1;
                targetHealth = target.GetComponent<Health>();
                targetHealth.currentHealth -= damageDealt;
            }
            else if ((animationTimeLeft <= 0 && targType.targetType == PlayerOrTarget.TargetType.Castle) && (dist <= 16 && dist >= 0))
            {
                animationTimeLeft = 1;
                targetHealth = target.GetComponent<Health>();
                targetHealth.currentHealth -= damageDealt;
            }
        }

        // Blending animations
        if (lastPos != transform.position)
        {
            animationSpeed = Mathf.Clamp01(animationSpeed + Time.deltaTime);
            anim.SetFloat("Blend", animationSpeed);
        }
        else
        {
            animationSpeed = Mathf.Clamp01(animationSpeed - Time.deltaTime);
            anim.SetFloat("Blend", animationSpeed);
        }
        lastPos = transform.position;
    }

    void OnCollisionStay(Collision other)
    {
        if(other.gameObject.tag == "Player" )
        {
           anim.SetTrigger("Attack");           
        }
    }        

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "BuildNode")
        {
            // attack towers 
            anim.SetTrigger("Attack");
        }
        else if (other.gameObject.tag == "Base")
        {
            // attack the base
            anim.SetTrigger("Attack");
        }
    }

    void OnTriggerEnter(Collider other)
    {        
        if (other.gameObject.tag == "Bullet")
        {
            bulletScript = other.gameObject.GetComponent<BulletControl>();
            Destroy(other.gameObject);
            myHealth.currentHealth -= bulletScript.damageDealt;
            audioPlayer.clip = (sounds[Random.Range(0, sounds.Length)]);
            audioPlayer.Play();
        }
    }

    void HealthUpdate()
    {
        healthBar.transform.position = transform.position + new Vector3(0, 1, 1);
        healthBar.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
        healthBarFill.fillAmount = myHealth.currentHealth / myHealth.startHealth;
        if (myHealth.currentHealth <= 0)
        {
            //Destroy enemy and play particle effects/animation
            //GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIManager>().GameOver();
            Destroy(gameObject);
        }
    }

    void PopulateStructureLists()
    {
        // hold all of the structures and their target scripts in temporary arrays        
        PlayerOrTarget[] tempStructTypes = GameObject.FindObjectsOfType<PlayerOrTarget>(); // get all objects with a certain script        
        GameObject[] tempStructures = new GameObject[tempStructTypes.Length];
        for (int i = 0; i < tempStructTypes.Length; i++)
        {
            tempStructures[i] = tempStructTypes[i].gameObject; // make an array of the game objects attached to the scripts
        }

        // now only populate the Lists we are going to use with targetable structures
        for (int i = 0; i < tempStructures.Length; i++)
        {
            if (tempStructTypes[i].targetType == PlayerOrTarget.TargetType.Battlement || tempStructTypes[i].targetType == PlayerOrTarget.TargetType.Castle)
            {
                structures.Add(tempStructures[i]);
                strucTypes.Add(tempStructTypes[i]);
            }
        }
    }
}
