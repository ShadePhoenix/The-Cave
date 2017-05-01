using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class SlowTower : MonoBehaviour {
    public GameObject healthBar;
    public Image healthBarFill;
    public int health = 100;
    float currentHealth;

    [Tooltip("How much the unit will cost to build")]
    public int conMatCost;
    [Tooltip("Cost to slow each enemy that enters its radius")]
    public int energyFireCost;



    // Use this for initialization
    void Start () {
        currentHealth = health;
    }

    // Update is called once per frame
    void Update () {
        HealthUpdate();

	}
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
                other.gameObject.GetComponent<NavMeshAgent>().speed = (other.gameObject.GetComponent<NavMeshAgent>().speed) / 2;
                Debug.Log(other.gameObject.GetComponent<NavMeshAgent>().speed);
        }
    }

    void OnTriggerExit(Collider other)
    {

        if (other.tag == "Enemy")
        {
            other.gameObject.GetComponent<NavMeshAgent>().speed = (other.gameObject.GetComponent<NavMeshAgent>().speed) *2;
            //Debug.Log(other.gameObject.GetComponent<NavMeshAgent>().speed);
        }


    }

    void HealthUpdate()
    {
        healthBar.transform.position = transform.position + new Vector3(0, 2, -2.5f);
        healthBar.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
        healthBarFill.fillAmount = currentHealth / health;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
