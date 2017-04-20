using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

//[RequireComponent(typeof())]

public class TurretAI : MonoBehaviour
{

    [Tooltip("The Object that will rotate")]
    public GameObject turretArm;
    [Tooltip("The Object that the bullets will fire from *Best to parent to Turret Head*")]
    public Transform projectileSpawn;
    [Tooltip("")]
    public GameObject projectile;

    //public GameObject bulletPrefab;

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

    GameObject target;

    float currentTargetDis = Mathf.Infinity;

    public LayerMask mask;
    
    public Collider[] enemiesInRange;

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RangeMonitor();
        Aim();
        Fire();
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
            Vector3 posDif = target.transform.position - transform.position;
            Vector3 turretRot = transform.rotation.eulerAngles;
            turretRot.y = (Mathf.Atan2(posDif.x, posDif.z) * Mathf.Rad2Deg);
            transform.localRotation = Quaternion.Euler(turretRot);
            turretArm.transform.LookAt(target.transform);
        }
    }

    bool fire = true;

    void Fire()
    {
        if (fire && target != null)
        {
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
}