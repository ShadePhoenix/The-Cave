using UnityEngine;
using System.Collections;

public class AIController : MonoBehaviour 
{
    [Tooltip("Add the player as a target for nav mesh.")]
    public Transform player;
    public Transform turret;
	private UnityEngine.AI.NavMeshAgent m_agent;

    public LayerMask mask;

    void Start () 
	{
		m_agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
	}	
	
	void Update () 
	{     
        if(playerController.playerAtBase == false)
        {
            m_agent.SetDestination(player.position);
        }
        else
        {
            m_agent.SetDestination(turret.position);
        }
        
	}
}
