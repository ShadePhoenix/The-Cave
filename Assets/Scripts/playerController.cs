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

    private Rigidbody rb;    
    static public bool playerAtBase = false;
    void Start ()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {       
        if(health <= 0)
        {
            Debug.Log("DEAD");
            Time.timeScale = 0;
        }
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
            Destroy(other);
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
