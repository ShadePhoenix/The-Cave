using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]

public class TurretAI : MonoBehaviour
{
    //[Tooltip("Starting Health of the Player Turret")]
    //public int health = 100;
    //float currentHealth;

    [Tooltip("The Object that will rotate")]
    public GameObject turretArm;
    [Tooltip("The Object that the bullets will fire from *Best to parent to Turret Head*")]
    public Transform projectileSpawn;
    [Tooltip("")]
    public GameObject projectile;
    //private int numProjectiles = 400; // number of projectiles to pool
    
    //private List<GameObject> bulletList = new List<GameObject>(); // pool of normal bullets   
    //private List<Rigidbody> bulletBodies = new List<Rigidbody>(); // pool of bullet bodies
    //private bool bulletGen = false; // have normal bullets been generated

    [Tooltip("How much the unit will cost to build")]
    public int conMatCost = 1;
    [Tooltip("")]
    public int energyFireCost = 1;
    [Tooltip("The range that it can target enemies")]
    public float fireRange = 100;
    [Tooltip("")]
    public float projectileSpeed;

    [Tooltip("Time between shots in seconds")]
    public float fireWait = 1;

    //public GameObject healthBar;
    //public Image healthBarFill;

    public GameObject target;

    public LayerMask mask;
    
    public Collider[] enemiesInRange;
    public AudioClip[] sounds;
    private AudioSource audioPlayer;

    //private Health myHealth;

    // Use this for initialization
    void Start()
    {
        //myHealth = gameObject.GetComponent<Health>();
        //currentHealth = health;
        audioPlayer = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //GenBullets();  // Generate a pool bullets if they haven't been already
        RangeMonitor();
        Aim();
        Fire();
        //HealthUpdate();
        
    }

    //Calculates which enemy in range has traveled the furthest and targets them
    void RangeMonitor()
    {
        enemiesInRange = Physics.OverlapSphere(transform.position, fireRange, mask);
        if (enemiesInRange.Length > 0)
        {
            foreach (Collider eCol in enemiesInRange)
            {
                target = eCol.gameObject;
            }
        }
        else
        {
            target = null;
        }  
    }

    //Rotates the turret head to face the target
    void Aim()
    {
        if (target != null)
        {
            float dis = Vector3.Distance(transform.position, target.transform.position);
            float travelTime = dis / projectileSpeed;
            Vector3 aimPoint = target.transform.position + travelTime * target.GetComponent<Rigidbody>().velocity;
            Vector3 posDif = aimPoint - transform.position;
            Vector3 turretRot = transform.rotation.eulerAngles;
            turretRot.y = (Mathf.Atan2(posDif.x, posDif.z) * Mathf.Rad2Deg);
            transform.localRotation = Quaternion.Euler(turretRot);
            turretArm.transform.LookAt(aimPoint);            
        }
    }

    bool fire = true;

    void Fire()
    {
        if ((fire && target != null))
        {
            UIManager.energy -= energyFireCost;
            GameObject lProjectile = Instantiate(projectile, projectileSpawn.position, Quaternion.Euler(projectileSpawn.transform.eulerAngles));
            lProjectile.GetComponent<Rigidbody>().AddForce(lProjectile.transform.forward * projectileSpeed, ForceMode.Impulse);
            int index = Random.Range(0, sounds.Length);
            audioPlayer.clip = (sounds[index]);
            audioPlayer.Play();
            StartCoroutine(FireWait(fireWait));
            fire = false;
        }
    }

    IEnumerator FireWait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        fire = true;
    }

    //void HealthUpdate()
    //{
    //    healthBar.transform.position = transform.position + new Vector3(0, 2, -2.5f);
    //    healthBar.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
    //    healthBarFill.fillAmount = myHealth.currentHealth / myHealth.startHealth;
    //    if (myHealth.currentHealth <= 0)
    //    {
    //        Destroy(gameObject);
    //    }
    //}    
}