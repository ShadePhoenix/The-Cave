using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JukeBox : MonoBehaviour {
    public AudioClip DaySong;
    public AudioClip NightSong;
    private AudioSource player;
    private bool cycle;
	// Use this for initialization
	void Start () {
        player = GetComponent<AudioSource>();
        cycle = DayNight.isNight;
        ChangeTrack(cycle);
	}
	
	// Update is called once per frame
	void Update () {
		if (cycle!= DayNight.isNight)
        {
            cycle = DayNight.isNight;
            ChangeTrack(cycle);
        }
	}
    void ChangeTrack(bool track)
    {
        if (track)
        {
            player.clip = NightSong;
            player.Play();
        }
        if (!track)
        {
            player.clip = DaySong;
            player.Play();
        }
    }
}
