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

    [Tooltip("Damage dealt to enemies when hit")]
    public int damage;

    public GameObject projectile;

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
        Aim();
        Fire();
        HealthUpdate();
    }
    bool playerActive;
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && Input.GetKeyDown(KeyCode.E))
            playerActive = !playerActive;
    }

    void Aim()
    {

            Vector2 posDif = Input.mousePosition - m_Camera.WorldToScreenPoint(transform.position);
            Vector3 playerRot = playerTurret.transform.rotation.eulerAngles;
            playerRot.y = Mathf.Atan2(posDif.x, posDif.y) * Mathf.Rad2Deg;
            playerTurret.transform.rotation = Quaternion.Euler(playerRot);
    }

    bool fire = true;
    void Fire()
    {
        if (fire && Input.GetButtonDown("Fire1") && !EventSystem.current.IsPointerOverGameObject())
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
