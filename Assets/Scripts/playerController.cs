using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour {

    [Tooltip("Speed of walk acceleration.")]
    public float walkSpeed = 3;
    [Tooltip("Max walking speed.")]
    public float maxWalkSpeed = 10;

    private Rigidbody rb;
    //private Input input;

    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {    
        Vector3 moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        // Debug.Log(moveDirection);
        // moveDirection = Camera.main.transform.TransformDirection(moveDirection);
        moveDirection.y = 0;

        rb.velocity += (moveDirection * walkSpeed);
        Debug.Log(rb.velocity);

        Vector3 velocity = rb.velocity;

        velocity.x = Mathf.Clamp(velocity.x, -maxWalkSpeed, maxWalkSpeed);
        velocity.z = Mathf.Clamp(velocity.z, -maxWalkSpeed, maxWalkSpeed);

        rb.velocity = velocity;   
    }
}
