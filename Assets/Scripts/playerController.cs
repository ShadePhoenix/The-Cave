using System.Collections;
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
    [Tooltip("The players health.")]
    public int health = 2;
    [Tooltip("Damage player takes from normal enemies.")]
    public int damageTakenNormal = 1;
    [Tooltip("How much of the global energy you use up while running per second.")]
    public int staminaDrain = 1;
    private float staminaDrainTimer = 1;
    //public GameObject gameOver;

    private float horizontal;
    private float vertical;
    private float speed = 0f;
    private Animator anim;
    private Vector3 lastPos;

    private Rigidbody rb;    
    static public bool playerAtBase = false;
    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        lastPos = transform.position;
        //gameOver.SetActive(false);

    }

    void Update()
    {       
        if(health <= 0)
        {
            Debug.Log("DEAD");
            //gameOver.SetActive(true);
            Time.timeScale = 0;
        }

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            transform.localEulerAngles = new Vector3(0, -180, 0);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            transform.localEulerAngles = new Vector3(0, 90, 0);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            transform.localEulerAngles = new Vector3(0, -90, 0);
        }
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

    void FixedUpdate()
    {
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        moveDirection.y = 0;
        if (Input.GetKey(KeyCode.LeftShift) && UIManager.energy > 0)
        {
            staminaDrainTimer -= Time.deltaTime;
            if (staminaDrainTimer <= 0)
            {
                UIManager.energy -= staminaDrain;
                staminaDrainTimer = 1;
            }

            rb.velocity += (moveDirection * runAcceleration);

            Vector3 velocity = rb.velocity;
            velocity.x = Mathf.Clamp(velocity.x, -maxRunSpeed, maxRunSpeed);
            velocity.z = Mathf.Clamp(velocity.z, -maxRunSpeed, maxRunSpeed);

            rb.velocity = velocity;
        }
        else
        {
            rb.velocity += (moveDirection * walkAcceleration);

            Vector3 velocity = rb.velocity;
            velocity.x = Mathf.Clamp(velocity.x, -maxWalkSpeed, maxWalkSpeed);
            velocity.z = Mathf.Clamp(velocity.z, -maxWalkSpeed, maxWalkSpeed);

            rb.velocity = velocity;
        }
    }

    

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Base Trigger")
        {           
            playerAtBase = true;
        }

        if (other.gameObject.tag == "Gold")
        {
            UIManager.gold += UIManager.goldVal;
            print("How much gold I have: " + UIManager.gold);
            Destroy(other.gameObject);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Base Trigger")
        {            
            playerAtBase = false;
        }
    }
}
