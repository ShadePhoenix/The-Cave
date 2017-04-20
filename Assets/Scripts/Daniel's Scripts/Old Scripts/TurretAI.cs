using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

//[RequireComponent(typeof())]

public class TurretAI : MonoBehaviour
{

    [Tooltip("The Object that will rotate")]
    public GameObject turretHead;
    [Tooltip("The Object that the bullets will fire from *Best to parent to Turret Head*")]
    public Transform barrel;

    //public GameObject bulletPrefab;

    [Tooltip("How much the unit will cost to build")]
    public int conMatCost;
    [Tooltip("Damage dealt to enemies when hit")]
    public int damage;
    [Tooltip("")]
    public int energyFireCost;
    [Tooltip("The range that it can target enemies")]
    public float fireRange;
    //[Tooltip("")]
    //public float bulletSpeed;
    [Tooltip("Time between shots in seconds")]
    public float fireWait;
    //[Tooltip("")]
    //public Image turretIcon;

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
            Vector3 turretRot = turretHead.transform.rotation.eulerAngles;
            turretRot.y = (Mathf.Atan2(posDif.x, posDif.z) * Mathf.Rad2Deg);
            turretHead.transform.rotation = Quaternion.Euler(turretRot);
        }
    }

    //Fires a raycast to damage enemies
    bool fire = true;
    void Fire()
    {
        if (fire && target != null)
        {
            RaycastHit hit;
            if(Physics.Raycast(barrel.position, turretHead.transform.forward, out hit, fireRange, mask))
            {
                //hit.collider.gameObject.GetComponent<EnemyAI>().TakeDamage(damage);
                //print(hit.collider.gameObject);
            }
            StartCoroutine(FireWait(fireWait));
            fire = false;
        }
    }

    //Adds a waiting time before the next shot can be fired
    IEnumerator FireWait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        fire = true;
    }
}