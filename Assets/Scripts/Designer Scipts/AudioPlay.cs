using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class AudioPlay : MonoBehaviour {
    private AudioSource player;
    public AudioClip[] effects;
	// Use this for initialization
	void Start () {
        player = GetComponent<AudioSource>();

	}
	
	// Update is called once per frame
	void Update () {
        player.clip = effects[(Random.Range(0, effects.Length))];
        player.Play();
	}
}
