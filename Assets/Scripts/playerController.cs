using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour {

    [Tooltip("Speed of walk acceleration.")]
    public float walkSpeed = 3;
    [Tooltip("Max walking speed.")]
    public float maxWalkSpeed = 10;

    private Rigidbody rb;
    [HideInInspector]
    static public bool playerAtBase = false;
    void Start ()
    {
        rb = GetComponent<Rigidbody>();
    }
		
	void FixedUpdate ()
    {    
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));        
        moveDirection.y = 0;
        rb.velocity += (moveDirection * walkSpeed);       

        Vector3 velocity = rb.velocity;
        velocity.x = Mathf.Clamp(velocity.x, -maxWalkSpeed, maxWalkSpeed);
        velocity.z = Mathf.Clamp(velocity.z, -maxWalkSpeed, maxWalkSpeed);

        rb.velocity = velocity;   
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Base Trigger")
        {           
            playerAtBase = true;
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
