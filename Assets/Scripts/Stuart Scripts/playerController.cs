﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour {

    [Tooltip("Speed of walk acceleration.")]
    public float walkAcceleration = 3;
    [Tooltip("Max walking speed.")]
    public float maxWalkSpeed = 10;
    [Tooltip("Speed of Run acceleration.")]
    public float runAcceleration = 4;
    [Tooltip("Max running speed.")]
    public float maxRunSpeed = 14;    
    private Health health;
    [Tooltip("Damage player takes from normal enemies.")]
    public int damageTakenNormal = 1;
    [Tooltip("How much of the global energy you use up while running per second.")]
    public int staminaDrain = 1;
    private float staminaDrainTimer = 1;
    [Tooltip("Attach the Game Over screen.")]
    public GameObject gameOver;

    private List<GameObject> targetList = new List<GameObject>();
    private float horizontal;
    private float vertical;
    private float speed = 0f;
    private Animator anim;
    private Vector3 lastPos;
    private float animationTimeLeft = 0.8f;

    private Rigidbody rb;    
    static public bool playerAtBase = false;
    static public bool playerActive = true;
    [HideInInspector]
    static public new CapsuleCollider collider;
    [HideInInspector]
    static public SkinnedMeshRenderer mesh;

    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        lastPos = transform.position;
        health = GetComponent<Health>();
        gameOver.SetActive(false);
        collider = gameObject.GetComponent<CapsuleCollider>();
        mesh = gameObject.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
    }

    void Update()
    {  
        if (playerActive)
        {
            if (health.currentHealth <= 0)
            {                
                gameOver.SetActive(true);
                Time.timeScale = 0;
            }

            RotatePlayer();
            Attack();
            Animate();           
        }        
        else
        {
            collider.enabled = false;
        }
    }

    void FixedUpdate()
    {
        if(playerActive)
        {
            Move();
        }       
    }

    void OnTriggerEnter(Collider other)
    {  
        if (other.gameObject.tag == "Gold")
        {
            UIManager.gold += UIManager.goldVal;            
            Destroy(other.gameObject);
        }

        if (other.gameObject.tag == "CrystalNode")
        {            
            Damage(other.gameObject);
        }

        if (other.gameObject.tag == "OreNode")
        {            
            Damage(other.gameObject);
        }

        if (other.gameObject.tag == "Enemy")
        {
            //Rigidbody body = other.gameObject.GetComponent<Rigidbody>();
            //body.isKinematic = false;
            //body.AddForce(-transform.right * 10, ForceMode.Impulse);



            Damage(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
              
    }

    void RotatePlayer()
    {
        // get horizontal and vertical direction from -1 to 1 of keyboard or game pad
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || horizontal < 0 /*&& rb.velocity.x < 0*/)
        {
            transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) || horizontal > 0 /* && rb.velocity.x > 0*/)
        {
            transform.localEulerAngles = new Vector3(0, -180, 0);
        }
        else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) || vertical > 0 /*&& rb.velocity.y > 0*/)
        {
            transform.localEulerAngles = new Vector3(0, 90, 0);
        }
        else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S) || vertical < 0 /*&& rb.velocity.y < 0*/)
        {
            transform.localEulerAngles = new Vector3(0, -90, 0);
        }
    }

    void Move()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        //Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        //moveDirection.y = 0;
        if (Input.GetKey(KeyCode.LeftShift) && UIManager.energy > 0)
        {
            staminaDrainTimer -= Time.deltaTime;
            if (staminaDrainTimer <= 0)
            {
                UIManager.energy -= staminaDrain;
                staminaDrainTimer = 1;
            }
            //rb.velocity += (moveDirection * runAcceleration);
            Vector3 velocity = rb.velocity;
            velocity.x += horizontal * runAcceleration;
            velocity.z += vertical * runAcceleration;
            velocity.x = Mathf.Clamp(velocity.x, -maxRunSpeed, maxRunSpeed);
            velocity.z = Mathf.Clamp(velocity.z, -maxRunSpeed, maxRunSpeed);

            rb.velocity = velocity;
        }
        else
        {
            //rb.velocity += (moveDirection * walkAcceleration);
            Vector3 velocity = rb.velocity;
            velocity.x += horizontal * runAcceleration;
            velocity.z += vertical * runAcceleration;
            velocity.x = Mathf.Clamp(velocity.x, -maxWalkSpeed, maxWalkSpeed);
            velocity.z = Mathf.Clamp(velocity.z, -maxWalkSpeed, maxWalkSpeed);
            rb.velocity = velocity;
        }
    }

    void Attack()
    {        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            anim.SetBool("Hammer", true);
            anim.SetTrigger("Attack");
            animationTimeLeft = 0.7f;
        }

        if (anim.GetBool("Hammer") == true)
        {
            animationTimeLeft -= Time.deltaTime;
            if (animationTimeLeft <= 0)
            {
                anim.SetBool("Hammer", false);
                gameObject.transform.GetChild(0).gameObject.SetActive(false);
                animationTimeLeft = 0;                
            }
        }              
    }

    void Damage(GameObject target)
    {               
        Health health = target.GetComponent<Health>();
        health.currentHealth -= 1;       
    }

    void Animate()
    {
        if (lastPos != transform.position)
        {
            speed = Mathf.Clamp01(speed + Time.deltaTime);
            anim.SetFloat("Blend", speed);
        }
        else
        {
            speed = Mathf.Clamp01(speed - Time.deltaTime);
            anim.SetFloat("Blend", speed);
        }
        lastPos = transform.position;
    }
}


