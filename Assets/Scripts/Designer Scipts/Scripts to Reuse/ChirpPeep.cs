using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChirpPeep : MonoBehaviour {
    public bool alwaysBeeping = false;
    public Sprite chirp;
    public Sprite peep;
    public float animationSpeed=1.0f;
    private float timer;
    private bool chirping;
    private bool peeping;


	// Use this for initialization
	void Start () {
        int chirpeep = Random.Range(0, 2);
        if (chirpeep == 0)
        {
            chirping = true;
            gameObject.GetComponent<Image>().sprite = chirp;
        }
        else
        {
            peeping = true;
            gameObject.GetComponent<Image>().sprite = peep;
        }
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (timer > animationSpeed)
        {
            timer = 0;
            if (chirping)
            {
                chirping = false;
                peeping = true;
                gameObject.GetComponent<Image>().sprite = peep;
            }
            else if (peeping)
            {
                peeping = false;
                chirping = true;
                gameObject.GetComponent<Image>().sprite = chirp;
            }
        }

	}
}
