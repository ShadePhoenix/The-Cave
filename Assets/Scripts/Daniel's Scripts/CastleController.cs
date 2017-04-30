using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CastleController : MonoBehaviour {

    Camera m_Camera;

    [Tooltip("Starting Health of the Player Turret")]
    public int health = 100;
    int currentHealth;
    [Tooltip("The Object that will rotate")]
    public GameObject playerTurret;
    [Tooltip("The Object that will rotate")]
    public GameObject turretArm;
    [Tooltip("The Object that the bullets will fire from. *Best to parent to Turret Head*")]
    public Transform projectileSpawn;

    public GameObject projectile;
    public GameObject player;
    public Transform playerArea;

    float fireWait = 1;

    public float projectileSpeed = 50;
    public Image healthBarFill;

    private bool triggered = false;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        currentHealth = health;
        m_Camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.E) && player.gameObject.activeSelf == false)
        //{
        //    player.gameObject.SetActive(true);
        //    player.transform.position = playerArea.position;
        //}

        if ((triggered && Input.GetKeyDown(KeyCode.E) ) && player.activeSelf == true)
        {
            player.SetActive(false);
            m_Camera.gameObject.GetComponent<Follow>().followObject = gameObject;
            gameObject.GetComponent<AudioListener>().enabled = true;
        }
        else if (Input.GetKeyDown(KeyCode.E) && player.activeSelf == false)
        {
            player.transform.position = playerArea.position;
            player.SetActive(true);
            m_Camera.gameObject.GetComponent<Follow>().followObject = player;
            gameObject.GetComponent<AudioListener>().enabled = false;
        }


        if (player.activeSelf == false)
        {
            Aim();
            Fire();
        }

        HealthUpdate();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            triggered = true;           
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            triggered = false;
        }
    }

    RaycastHit hit;
    void Aim()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out hit, terrainMask))
        {
            playerTurret.transform.forward = hit.point - transform.position;
            Vector3 angles = playerTurret.transform.eulerAngles;
            angles.x = angles.z = 0;
            playerTurret.transform.eulerAngles = angles;
            turretArm.transform.forward = hit.point - transform.position;
        }
    }

    bool fire = true;
    public LayerMask terrainMask;
    void Fire()
    {
        if (fire && Input.GetButton("Fire1") && !EventSystem.current.IsPointerOverGameObject() && hit.collider.tag != "BuildNode")
        {
            GameObject bullet = Instantiate(projectile, projectileSpawn.position, Quaternion.Euler(projectileSpawn.transform.eulerAngles));
            bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * projectileSpeed, ForceMode.Impulse);
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
        healthBarFill.fillAmount = currentHealth / health;
        if (currentHealth <= 0)
        {
            //GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIManager>().GameOver();
        }
    }
}
