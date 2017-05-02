using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildNode : MonoBehaviour {

    [HideInInspector]
    public GameObject turret;
    public bool allowBuild = true;

    public GameObject healthBar;
    public Image healthBarFill;

    private Health myHealth;
    private PlayerOrTarget type;

    private GameObject gc;
    private AITargets aiTargets;

    // Use this for initialization
    void Start()
    {
        type = gameObject.GetComponent<PlayerOrTarget>();
        if (type.targetType != PlayerOrTarget.TargetType.Non_Target)
        {
            myHealth = gameObject.GetComponent<Health>();
        }

        gc = GameObject.FindObjectOfType<Main>().gameObject;
        aiTargets = gc.GetComponent<AITargets>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(type.targetType != PlayerOrTarget.TargetType.Non_Target)
        {
            healthBar.transform.position = transform.position + new Vector3(0, 2, -2.5f);
            healthBar.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));            
            healthBarFill.fillAmount = myHealth.currentHealth / myHealth.startHealth;
            if (myHealth.currentHealth <= 0 && turret != null && !allowBuild)
            {
                aiTargets.RemoveTarget(gameObject);
                Destroy(turret);
                allowBuild = true;
            }
            if(turret == null && allowBuild)
            {
                myHealth.currentHealth = 0;
            }
        }
       
    }
}
