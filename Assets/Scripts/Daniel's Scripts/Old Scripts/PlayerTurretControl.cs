using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerTurretControl : MonoBehaviour {

    Camera m_Camera;

    [Tooltip("Starting Health of the Player Turret")]
    public float health = 100;
    [Tooltip("The Object that will rotate")]
    public GameObject turretHead;
    [Tooltip("The Object that the bullets will fire from. *Best to parent to Turret Head*")]
    public Transform barrel;

    [Tooltip("Damage dealt to enemies when hit")]
    public int damage;

    public GameObject bulletPrefab;

    float currentHealth;

    float fireWait = 1;

    public float bulletSpeed = 50;

    public Image healthBarFill;

    // Use this for initialization
    void Start ()
    {
        currentHealth = health;
        m_Camera = Camera.main;
        currentHealth = health;
	}
	
	// Update is called once per frame
	void Update ()
    {
        Aim();
        Fire();
        HealthUpdate(); 
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            currentHealth = currentHealth - other.GetComponent<EnemyAI>().damageToPlayer;
            Destroy(other.gameObject);
        }
    }

    void Aim()
    {
        if (!UIManager.uiMode)
        {
            Vector2 posDif = Input.mousePosition - m_Camera.WorldToScreenPoint(transform.position);
            Vector3 playerRot = turretHead.transform.rotation.eulerAngles;
            playerRot.y = Mathf.Atan2(posDif.x, posDif.y) * Mathf.Rad2Deg;
            turretHead.transform.rotation = Quaternion.Euler(playerRot);
        }
    }

    bool fire = true;

    void Fire()
    {
        if (fire && Input.GetButtonDown("Fire1") && !EventSystem.current.IsPointerOverGameObject() && !UIManager.uiMode)
        {
            GameObject bullet = Instantiate(bulletPrefab, barrel.position, Quaternion.Euler(turretHead.transform.eulerAngles));
            bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * bulletSpeed, ForceMode.Impulse);
            barrel.GetComponent<ParticleSystem>().Play();
            float waitTime = fireWait;
            StartCoroutine(FireWait(waitTime));
            fire = false;
            waitTime = 0.2f;
            StartCoroutine(FireWait(waitTime));
        }
    }

    IEnumerator FireWait(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (waitTime == fireWait)
            fire = true;
        else
            barrel.GetComponent<ParticleSystem>().Stop();
    }

    void HealthUpdate()
    {
        healthBarFill.fillAmount = currentHealth / health;
        if (currentHealth <= 0)
        {
            GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIManager>().GameOver();
        }
    }
}