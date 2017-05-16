using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ParticleController : MonoBehaviour {

    public AudioClip sound;
    private AudioSource audioPlayer;
    //private ParticleSystem ps;
    private float timer;
    // Use this for initialization

    private void Awake()
    {
        //gameObject.particleSystem.
    }

    void Start () {
        //ps = gameObject.GetComponent<ParticleSystem>();
        audioPlayer = gameObject.GetComponent<AudioSource>();
        audioPlayer.clip = sound;
        audioPlayer.Play();
	}
	
	// Update is called once per frame
	void Update () {
        /*if (!ps.IsAlive())
        {
            Destroy(gameObject);
        }*/
        timer += Time.deltaTime;
        if (timer > 1)
        {
            Destroy(gameObject);
        }
	}
}
