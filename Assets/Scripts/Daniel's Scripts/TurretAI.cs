using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

//[RequireComponent(typeof())]

public class TurretAI : MonoBehaviour
{
    [Tooltip("Starting Health of the Player Turret")]
    public int health = 100;
    float currentHealth;
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
    public int conMatCost;
    [Tooltip("Damage dealt to enemies when hit")]
    public int damage;
    [Tooltip("")]
    public int energyFireCost;
    [Tooltip("The range that it can target enemies")]
    public float fireRange;
    [Tooltip("")]
    public float projectileSpeed;

    [Tooltip("Time between shots in seconds")]
    public float fireWait;

    public GameObject healthBar;
    public Image healthBarFill;

    GameObject target;

    float currentTargetDis = Mathf.Infinity;

    public LayerMask mask;
    
    public Collider[] enemiesInRange;

    // Use this for initialization
    void Start()
    {
        currentHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        //GenBullets();  // Generate a pool bullets if they haven't been already
        RangeMonitor();
        Aim();
        Fire();
        HealthUpdate();
        
    }

    //Calculates which enemy in range has traveled the furthest and targets them
    void RangeMonitor()
    {
        currentTargetDis = Mathf.Infinity;
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
            currentTargetDis = Mathf.Infinity;
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

            //Vector3 posDif = target.transform.position - transform.position;
            //Vector3 turretRot = transform.rotation.eulerAngles;
            //turretRot.y = (Mathf.Atan2(posDif.x, posDif.z) * Mathf.Rad2Deg);
            //transform.localRotation = Quaternion.Euler(turretRot);
            //turretArm.transform.LookAt(target.transform);
        }
    }

    bool fire = true;

    void Fire()
    {
        if ((fire && target != null))
        {
            //for (int i = 0; i < numProjectiles; i++)
            //{
            //    if (bulletList[i].gameObject.activeSelf == false)
            //    {
            //        bulletList[i].gameObject.SetActive(true); // reactivate the bullet
            //        // set its position
            //        bulletList[i].transform.position = new Vector3(projectileSpawn.position.x, projectileSpawn.position.y, projectileSpawn.position.z); 
            //        bulletList[i].transform.eulerAngles = projectileSpawn.transform.eulerAngles; // set its ange
            //        bulletBodies[i].AddForce(bulletList[i].transform.up * projectileSpeed, ForceMode.Impulse);  // add force on the matching rigidbody                  
            //        StartCoroutine(FireWait(fireWait));
            //        fire = false;                   
            //        return;
            //    }
            //}
            GameObject lProjectile = Instantiate(projectile, projectileSpawn.position, Quaternion.Euler(projectileSpawn.transform.eulerAngles));
            lProjectile.GetComponent<Rigidbody>().AddForce(lProjectile.transform.up * projectileSpeed, ForceMode.Impulse);
            StartCoroutine(FireWait(fireWait));
            fire = false;
        }
    }

    IEnumerator FireWait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        fire = true;
    }

    void HealthUpdate()
    {
        healthBar.transform.position = transform.position + new Vector3(0, 2, -2.5f);
        healthBar.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
        healthBarFill.fillAmount = currentHealth / health;
        if (currentHealth <= 0)
        {
            //Destroy Turret and play particle effects
            //GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIManager>().GameOver();
        }
    }

    //void GenBullets()
    //{
    //    if(bulletGen == false)
    //    {
    //        bulletGen = true;
    //        for (int i = 0; i < numProjectiles; i++)
    //        {
    //            bulletList.Add((GameObject)Instantiate(projectile, new Vector3(-10, -10, 0), Quaternion.identity));
    //            bulletList[i].gameObject.SetActive(false);
    //            bulletBodies.Add(bulletList[i].GetComponent<Rigidbody>());
    //        }
    //    }        
    //}
}