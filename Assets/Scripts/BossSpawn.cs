using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawn : MonoBehaviour {

    [Tooltip("Chance for boss to spawn out of 100.")]
    public int spawnChance = 1;
    private int currentSpawnChance;
    [Tooltip("Every night that a boss does not spaw, the chance for him to spawn increases by this amount, up to 100.")]
    public int spawnChanceIncrease = 1;
    [Tooltip("Reference for the Boss. Attach the game object.")]
    public GameObject boss;
    [Tooltip("How many nights must pass before a boss spawns.")]
    public int nightsBeforeSpawn = 0;

    private enum CanSpawn
    {
        Ready,
        Cannot,
        Waiting
    }

    CanSpawn canSpawn = CanSpawn.Cannot;

    // Use this for initialization
    void Start ()
    {
        currentSpawnChance = spawnChance;
        currentSpawnChance = Mathf.Clamp(currentSpawnChance, 0, 100);
        spawnChanceIncrease = Mathf.Clamp(spawnChanceIncrease, 1, 100);
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (canSpawn == CanSpawn.Ready && DayNight.nightNumber >= nightsBeforeSpawn)
        {
            canSpawn = CanSpawn.Waiting; // wait for the next night for a chance to spawn again
            int rand = Random.Range(1, 100);

            if(rand > 0 && rand <= currentSpawnChance)
            {
                currentSpawnChance = spawnChance;
                Instantiate(boss, transform.position, Quaternion.identity);
            }
        }

        if(DayNight.isNight == false)
        {
            canSpawn = CanSpawn.Cannot; // Cannot spawn in day
        }
        else if(DayNight.isNight == true && canSpawn == CanSpawn.Waiting) // If it's day time and waiting to be spawn ready
        {
            currentSpawnChance += spawnChanceIncrease;
            canSpawn = CanSpawn.Ready; // Ready to spawn. It is night time now
        }
	}
}
