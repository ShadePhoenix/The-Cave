using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CastleController : MonoBehaviour {

    Camera m_Camera;

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
    private Health myHealth;
    private Follow followScript;
    [Tooltip("Attach the Game Over screen.")]
    public GameObject gameOver;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        myHealth = gameObject.GetComponent<Health>();
        //currentHealth = health;
        m_Camera = Camera.main;
        followScript = m_Camera.gameObject.GetComponent<Follow>();
        //gameOver = FindObjectOfType<GameOver>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {     
        if ((triggered && Input.GetKeyDown(KeyCode.E) ) && playerController.playerActive == true)
        {
            playerController.playerActive = false;
            playerController.collider.enabled = false;
            playerController.mesh.enabled = false; 
            followScript.followObject = gameObject;
            followScript.followingPlayer = false;
            //gameObject.GetComponent<AudioListener>().enabled = true;
        }
        else if (Input.GetKeyDown(KeyCode.E) && playerController.playerActive == false)
        {
            player.transform.position = playerArea.position;
            playerController.playerActive = true;
            playerController.collider.enabled = true;
            playerController.mesh.enabled = true;
            followScript.followObject = player;
            followScript.followingPlayer = true;
           // gameObject.GetComponent<AudioListener>().enabled = false;
        }

        if (playerController.playerActive == false)
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
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, terrainMask))
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
        if (fire && Input.GetButton("Fire1") && !EventSystem.current.IsPointerOverGameObject())
        {
            GameObject bullet = Instantiate(projectile, projectileSpawn.position, Quaternion.identity);
            bullet.transform.LookAt(hit.point);
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
        healthBarFill.fillAmount = myHealth.currentHealth / myHealth.startHealth;
        if (myHealth.currentHealth <= 0)
        {
            gameOver.SetActive(true);
            //GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIManager>().GameOver();
        }
    }
}
