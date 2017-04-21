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

    [Tooltip("Damage dealt to enemies when hit")]
    public int damage;

    public Transform playerArea;

    float fireWait = 1;

    public float projectileSpeed = 50;

    public Image healthBarFill;

    // Use this for initialization
    void Start()
    {
        currentHealth = health;
        m_Camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerActive)
        {
            Aim();
            Fire();
        }
        HealthUpdate();
    }

    static public bool playerActive = true;
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && Input.GetKeyDown(KeyCode.E))
        {
            playerActive = !playerActive;
            //Move Player to PlayerArea in the castle
            //Lerp Camera to new position : New to figure out what script should handle camera movement
            //Deactivate player. Either SetActive(False) or disable all movement.
        }
    }

    void Aim()
    {
            Vector2 posDif = Input.mousePosition - m_Camera.WorldToScreenPoint(transform.position);
            Vector3 playerRot = playerTurret.transform.rotation.eulerAngles;
            playerRot.y = Mathf.Atan2(posDif.x, posDif.y) * Mathf.Rad2Deg;
            playerTurret.transform.rotation = Quaternion.Euler(playerRot);
    }

    bool fire = true;
    public LayerMask terrainMask;
    void Fire()
    {
        if (fire && Input.GetButtonDown("Fire1") && !EventSystem.current.IsPointerOverGameObject())
        {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, terrainMask))
                {
                    turretArm.transform.LookAt(hit.point);
                }
            GameObject bullet = Instantiate(projectile, projectileSpawn.position, Quaternion.Euler(projectileSpawn.transform.eulerAngles));
            bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.up * projectileSpeed, ForceMode.Impulse);
            StartCoroutine(FireWait(fireWait));
            fire = false;
            UIManager.uiState = UIManager.UIState.Build;
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
