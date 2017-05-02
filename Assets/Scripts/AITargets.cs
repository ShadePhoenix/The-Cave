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
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Init()
    {
        structures.Clear();
        strucTypes.Clear();
        targHealth.Clear();

        // Add the players castle to the list
        PlayerOrTarget[] tempStructTypes = GameObject.FindObjectsOfType<PlayerOrTarget>(); // get all objects with a certain script        
        GameObject[] tempStructures = new GameObject[tempStructTypes.Length];
        Health[] tempHealth = new Health[tempStructTypes.Length];
        for (int i = 0; i < tempStructTypes.Length; i++)
        {
            tempStructures[i] = tempStructTypes[i].gameObject; // make an array of the game objects attached to the scripts
            tempHealth[i] = tempStructures[i].gameObject.GetComponent<Health>();
        }

        for (int i = 0; i < tempStructures.Length; i++)
        {            
            if (tempStructTypes[i].targetType == PlayerOrTarget.TargetType.Castle)
            {                
                structures.Add(tempStructures[i]);
                strucTypes.Add(tempStructTypes[i]);
                targHealth.Add(tempHealth[i]);
            }
        }

        PrintTargets();
    }

    public void AddTarget(GameObject obj)
    {
        PlayerOrTarget tmpTarg = obj.GetComponent<PlayerOrTarget>();
        Health tmpHealth = obj.GetComponent<Health>();

        structures.Add(obj);
        strucTypes.Add(tmpTarg);
        targHealth.Add(tmpHealth);

        PrintTargets();
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

        PrintTargets();
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

    //void PopulateStructureLists()
    //{
    //    if (structures.Count > 0)
    //    {
    //        structures.Clear();
    //        strucTypes.Clear();
    //    }

    //    // hold all of the structures and their target scripts in temporary arrays
    //    PlayerOrTarget[] tempStructTypes = GameObject.FindObjectsOfType<PlayerOrTarget>(); // get all objects with a certain script        
    //    GameObject[] tempStructures = new GameObject[tempStructTypes.Length];
    //    Health structuretHealth;
    //    for (int i = 0; i < tempStructTypes.Length; i++)
    //    {
    //        tempStructures[i] = tempStructTypes[i].gameObject; // make an array of the game objects attached to the scripts
    //    }

    //    // now only populate the Lists we are going to use with targetable structures
    //    for (int i = 0; i < tempStructures.Length; i++)
    //    {
    //        structuretHealth = tempStructures[i].GetComponent<Health>();
    //        if (tempStructTypes[i].targetType == PlayerOrTarget.TargetType.Battlement || tempStructTypes[i].targetType == PlayerOrTarget.TargetType.Castle)
    //        {
    //            if (structuretHealth.currentHealth > 0)
    //            {
    //                structures.Add(tempStructures[i]);
    //                strucTypes.Add(tempStructTypes[i]);
    //            }
    //        }
    //    }
    //}
}
