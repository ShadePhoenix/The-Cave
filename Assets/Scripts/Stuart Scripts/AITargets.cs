using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITargets : MonoBehaviour {

    private List<GameObject> structures = new List<GameObject>();
    private List<PlayerOrTarget> strucTypes = new List<PlayerOrTarget>(); // the types of the structures, whether targetable or not
    private List<Health> targHealth = new List<Health>(); // health of the structures

    private GameObject player;
    private PlayerOrTarget playerType;
    private Health playerHealth;
    
                                      
    void Start ()
    {
        player = FindObjectOfType<playerController>().gameObject;
        playerHealth = player.GetComponent<Health>();
        playerType = player.GetComponent<PlayerOrTarget>();
        Init();
	}

    public void Init()
    {
        structures.Clear();
        strucTypes.Clear();
        targHealth.Clear();

        GameObject castle = FindObjectOfType<CastleController>().gameObject;
        PlayerOrTarget castleType = castle.GetComponent<PlayerOrTarget>();
        Health castleHealth = castle.GetComponent<Health>();

        structures.Add(castle);
        strucTypes.Add(castleType);
        targHealth.Add(castleHealth);

        //PrintTargets();
    }

    public void AddTarget(GameObject obj)
    {
        PlayerOrTarget tmpTarg = obj.GetComponent<PlayerOrTarget>();
        Health tmpHealth = obj.GetComponent<Health>();

        structures.Add(obj);
        strucTypes.Add(tmpTarg);
        targHealth.Add(tmpHealth);

        //PrintTargets();
    }

    public void RemoveTarget(GameObject target)
    {
        if (structures.Count > 0)
        {
            GameObject searchObj;
            for (int i = 0; i < structures.Count; i++)
            {
                // set the temporary object to the current object in the list
                searchObj = structures[i];
                if (searchObj == target) // if they match, remove the object and it's scripts from all lists
                {
                    structures.RemoveAt(i);
                    strucTypes.RemoveAt(i);
                    targHealth.RemoveAt(i);
                }
            }
        }

        //PrintTargets();
    }

    // returns a random structure to target
    public GameObject GetTarget()
    {
        int rStruct = Random.Range(0, structures.Count - 1);        
        return structures[rStruct]; 
    }

    // returns the type of the object
    public PlayerOrTarget GetType(GameObject target)
    {
        GameObject search;

        for(int i = 0; i < structures.Count; i++)
        {
            search = structures[i];
            if (search == target)
            {
                return strucTypes[i];
            }            
        }
        return null;
    }    

    // returns the health of the object
    public Health GetHealth(GameObject target)
    {
        GameObject search;

        for (int i = 0; i < structures.Count; i++)
        {
            search = structures[i];
            if (search == target)
            {
                return targHealth[i];
            }
        }
        return null;
    }

    public GameObject GetPlayer()
    {
        return player;
    }

    public PlayerOrTarget GetPlayerType()
    {
        return playerType;
    }

    public Health GetPlayerHealth()
    {
        return playerHealth;
    }

    void PrintTargets()
    {
        Main.ClearConsole();

        if (structures.Count > 0)
        {
            print("Printing the list of structures the AI can target.");

            for (int i = 0; i < structures.Count; i++)
            {
                print(strucTypes[i].targetType);
            }
        }        
    }
}
