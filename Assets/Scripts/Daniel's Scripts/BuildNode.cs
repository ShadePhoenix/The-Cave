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

    // Use this for initialization
    void Start ()
    {
        myHealth = gameObject.GetComponent<Health>();
        type = gameObject.GetComponent<PlayerOrTarget>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(type.targetType != PlayerOrTarget.TargetType.Non_Target)
        {
            healthBar.transform.position = transform.position + new Vector3(0, 2, -2.5f);
            healthBar.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
            healthBarFill.fillAmount = myHealth.currentHealth / myHealth.startHealth;
            if (myHealth.currentHealth <= 0 && gameObject.transform.childCount > 0)
            {                
                Destroy(gameObject.transform.GetChild(0));                                             
            }
            if(gameObject.transform.childCount <= 0)
            {
                myHealth.currentHealth = 0;
            }
        }
       
    }
}
