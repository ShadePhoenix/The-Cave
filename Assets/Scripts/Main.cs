using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour {

    [Tooltip("Damage of balista arrows.")]
    public int balistaDamage = 1;
    static public int s_balistaDamage;
    [Tooltip("Damage of cannon balls.")]
    public int cannonDamage = 2;
    static public int s_cannonDamage;
    [Tooltip("Damage of sniper bullets.")]
    public int sniperDamage = 4;
    static public int s_sniperDamage;
    public int playerBulletDamage = 5;
    static public int s_playerBulletDamage;

    // Use this for initialization
    void Start () {
        s_balistaDamage = balistaDamage;
        s_cannonDamage = cannonDamage;
        s_sniperDamage = sniperDamage;
        s_playerBulletDamage = playerBulletDamage;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
