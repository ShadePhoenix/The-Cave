using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour {

    [Tooltip("Choose the X position you want bullets to disappear at. Automatically sets the opposite negative value.")]
    public float xVanishPosition = 1000;
    [Tooltip("Choose the Y position you want bullets to disappear at. Automatically sets the opposite negative value.")]
    public float yVanishPosition = 400;
    [Tooltip("Choose the Z position you want bullets to disappear at. Automatically sets the opposite negative value.")]
    public float zVanishPosition = 1000;

    [Tooltip("Choose the Z position you want bullets to disappear at. Automatically sets the opposite negative value.")]
    public int damageDealt = 1;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(transform.position.x >= xVanishPosition || transform.position.x <= -xVanishPosition)
        {
            Destroy(gameObject);
        }
        else if (transform.position.y >= yVanishPosition || transform.position.y <= -yVanishPosition)
        {
            Destroy(gameObject);
        }
        else if (transform.position.z >= zVanishPosition || transform.position.y <= -zVanishPosition)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Terrain")
        {
            Destroy(gameObject);
        }
    }
}
