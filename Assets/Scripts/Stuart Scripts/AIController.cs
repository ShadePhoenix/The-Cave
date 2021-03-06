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

    [Tooltip("Image for the health bar.")]
    public Image healthBarFill;
    public GameObject healthBar;

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
    [Tooltip("The Gold prefab to spawn when enemy is killed.")]
    public GameObject gold;

    [HideInInspector]
    public bool targeted = false;
    private Animator anim;
    private GameObject hero;   
    private GameObject target;
    private PlayerOrTarget targType; // what kind of target is it
    private NavMeshAgent m_agent;         
    private bool structureTargeted = false;
    private bool playerTargeted = false;

   
    public float animationTimeLeft = 1;
    private float animationSpeed = 0f;
    private Vector3 lastPos;

    

    private Health targetHealth;
    private Health myHealth;
    private GameObject gc;
    private AITargets aiTargets;

    [Tooltip("How long enemies are knocked back for.")]
    public float knockBackTotalTime = 1;
    private float knockBackLeft; 

    //private Transform marker;
    private Rigidbody body;

    void Start () 
	{
        knockBackLeft = knockBackTotalTime;

		m_agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        hero = GameObject.FindGameObjectWithTag("Player");        
        anim = GetComponent<Animator>();
        myHealth = GetComponent<Health>();
        lastPos = transform.position;        
        target = hero;
        targetHealth = target.GetComponent<Health>();
        audioPlayer = gameObject.GetComponent<AudioSource>();

        gc = GameObject.FindObjectOfType<Main>().gameObject;
        aiTargets = gc.GetComponent<AITargets>();

        StartCoroutine(DayDamage());

        body = GetComponent<Rigidbody>();
    }		
	void Update ()
    {       
        HealthUpdate();

        if(playerController.playerActive == true)
        {
            structureTargeted = false;
            SetTarget();
        }

        if ((targetHealth.currentHealth <= 0 || (structureTargeted == false && playerController.playerActive == false)))
        {
            structureTargeted = false;
            SetTarget();
        }
        else
        {
            Attack();
        }        
        
        Animate();        
        if(body.isKinematic == false)
        {
            knockBackLeft -= Time.deltaTime;

            if(knockBackLeft <= 0)
            {
                knockBackLeft = knockBackTotalTime;
                body.isKinematic = true;
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        Rigidbody otherBody = other.gameObject.GetComponent<Rigidbody>();
        if(other.gameObject.tag == "Player" && anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {          
           anim.SetTrigger("Attack");           
        }
        //if (other.gameObject.tag == "Enemy" && otherBody.isKinematic == true)
        //{            
        //    body.isKinematic = false;
        //    body.AddForce(other.rigidbody.velocity / 2, ForceMode.Impulse);            
        //}
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
        if (other.gameObject.tag == "BallistaBullet")
        {            
            Destroy(other.gameObject);
            myHealth.currentHealth -= Main.s_balistaDamage;
            audioPlayer.clip = (sounds[Random.Range(0, sounds.Length)]);
            audioPlayer.Play();
        }
        if (other.gameObject.tag == "CannonBullet")
        {            
            Destroy(other.gameObject);
            myHealth.currentHealth -= Main.s_cannonDamage;
            audioPlayer.clip = (sounds[Random.Range(0, sounds.Length)]);
            audioPlayer.Play();
        }
        if (other.gameObject.tag == "SniperBullet")
        {           
            Destroy(other.gameObject);
            myHealth.currentHealth -= Main.s_sniperDamage;
            audioPlayer.clip = (sounds[Random.Range(0, sounds.Length)]);
            audioPlayer.Play();
        }
        if (other.gameObject.tag == "PlayerBullet")
        {            
            Destroy(other.gameObject);
            myHealth.currentHealth -= Main.s_playerBulletDamage;
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
            //print("An enemy died");
            //spawns particle effect, and makes sure that the particle effect is its own object
            GameObject particles = Instantiate(deathEffect, transform.position, transform.rotation);
            particles.transform.parent = null;
            //Instantiate(gold, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }      
    }

    public void SetTarget()
    {
        if (playerController.playerActive == true)
        {            
            if (playerTargeted == false)
            {               
                target = aiTargets.GetPlayer();
                targType = aiTargets.GetPlayerType();
                targetHealth = aiTargets.GetPlayerHealth();

                // Get the targets position and check if the position is valid. Navmesh hit is required when objects may appare on unwalkable surfaces, or the target may be an object
                // baked into the mesh with unwalkable area around it. This will then check to see if it can come up to the edge of the unwalkable area
                Vector3 targetPos = target.transform.position;
                NavMeshHit myNavHit;
                if (targType.targetType == PlayerOrTarget.TargetType.Player && NavMesh.SamplePosition(targetPos, out myNavHit, 1, -1))
                {
                    // swap between the player or the structures being targeted
                    playerTargeted = true;
                    structureTargeted = false;

                    targetPos = myNavHit.position;                    
                    m_agent.SetDestination(targetPos); // set the enemies (thi objects) destination
                }                
            }
            else
            {
                Vector3 targetPos = target.transform.position;
                NavMeshHit myNavHit;
                if (targType.targetType == PlayerOrTarget.TargetType.Player && NavMesh.SamplePosition(targetPos, out myNavHit, 1, -1))
                {      
                    targetPos = myNavHit.position;
                    m_agent.SetDestination(targetPos);
                }                
            }       
        }
        else
        {           
            playerTargeted = false;

            while (structureTargeted == false)
            {       
                target = aiTargets.GetTarget();
                targType = aiTargets.GetType(target);
                targetHealth = aiTargets.GetHealth(target);
                
                Vector3 targetPos = target.transform.position;
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
                structureTargeted = true;                   
            }
        }
    }

    public void Attack()
    {
        // if the attack animation is playing, see if 1 second has passed and then do a distance check on the objects to be attacked
        // If the attack animation is playing, countdown from a timer, and when the timer is 0, do a distance check with the target to see if it can be damaged        
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            //print("The attack animation is playing now.");
            animationTimeLeft -= Time.deltaTime;
            float dist = Vector3.Distance(transform.position, target.transform.position);
            if ((animationTimeLeft <= 0 && targType.targetType == PlayerOrTarget.TargetType.Player) && (dist <= 2 && dist >= -2))
            {
                //print("The player should be getting hurt now");
                animationTimeLeft = 1;               
                targetHealth.currentHealth -= damageDealt;
            }
            else if ((animationTimeLeft <= 0 && targType.targetType == PlayerOrTarget.TargetType.Battlement) && (dist <= 7 && dist >= 0))
            {
                animationTimeLeft = 1;                
                targetHealth.currentHealth -= damageDealt;
            }
            else if ((animationTimeLeft <= 0 && targType.targetType == PlayerOrTarget.TargetType.Castle) && (dist <= 20 && dist >= 0))
            {
                animationTimeLeft = 1;                
                targetHealth.currentHealth -= damageDealt;
            }            
        }
    }

    public void Animate()
    {
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

    IEnumerator DayDamage()
    {
        while(true)
        {
            if(!DayNight.isNight)
            {
                myHealth.currentHealth--;
            }            
            yield return new WaitForSeconds(1);
        }
    }
}
