using UnityEngine;
using System.Collections;

public class AIController : MonoBehaviour 
{
    // [Tooltip("Add the player as a target for nav mesh.")]
    //public Transform player;
    [Tooltip("The range that it can target the players base from. There should always be some object in range or they will wander aimlessly.")]
    public float targetRange;

    private GameObject hero;
    private GameObject turret;
    private GameObject target;
    private UnityEngine.AI.NavMeshAgent m_agent;
    public LayerMask mask;
   
    private Collider[] structuresInRange;
    private float currentTargetDis = Mathf.Infinity;

    private GameObject[] structures;

    private bool targetStructureSet = false;

    void Start () 
	{
		m_agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        hero = GameObject.FindGameObjectWithTag("Player");
        turret = GameObject.FindGameObjectWithTag("Base");
        structures = GameObject.FindGameObjectsWithTag("Base");
    }	
	
	void Update () 
	{
        //RangeMonitor();

        if (playerController.playerAtBase == false)
        {
            targetStructureSet = false;
            m_agent.SetDestination(hero.transform.position);
        }
        else
        {
            if(targetStructureSet == false)
            {
                targetStructureSet = true;
                int rStruct = Random.Range(0, structures.Length);
                m_agent.SetDestination(structures[rStruct].transform.position);
            }
            
        }
        
	}
    //void RangeMonitor()
    //{
    //    currentTargetDis = Mathf.Infinity;
    //    structuresInRange = Physics.OverlapSphere(transform.position, targetRange, mask);
    //    if (structuresInRange.Length > 0)
    //    {
    //        float shortestDistance = 200000;
    //        float currentDistance = 0;
    //        foreach (Collider eCol in structuresInRange)
    //        {
    //            currentDistance = Vector3.Distance(transform.position, target.transform.position);
    //            if(currentDistance < shortestDistance)
    //            {
    //                shortestDistance = currentDistance;
    //            }
    //        }
    //        foreach (Collider eCol in structuresInRange)
    //        {
    //            currentDistance = Vector3.Distance(transform.position, target.transform.position);
    //            if (currentDistance == shortestDistance)
    //            {
    //                target = eCol.gameObject;
    //            }
    //        }
    //    }
    //    else
    //    {
    //        target = null;
    //        currentTargetDis = Mathf.Infinity;
    //    }
    //}
}
