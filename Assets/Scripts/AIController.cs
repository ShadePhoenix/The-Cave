using UnityEngine;
using System.Collections;

public class AIController : MonoBehaviour 
{
    public Transform player;
	private UnityEngine.AI.NavMeshAgent m_agent;
	// Use this for initialization
	void Start () 
	{
		m_agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () 
	{     
        m_agent.SetDestination(player.position);
	}
}
