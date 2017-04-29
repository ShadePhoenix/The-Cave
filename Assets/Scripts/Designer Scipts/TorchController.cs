using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchController : MonoBehaviour {
    public float visionSteps=250;
    public GameObject[] torches;
    public float maxBrightness=1;
    public float maxRange=20;
    public float energy;
    private float oldEnergy;

	// Use this for initialization
	void Start () {
        energy = UIManager.energy;
        //energy = 400;
        for (int i = 0; i < torches.Length; i++)
        {
                LightSwitch(i);           
        }
    }

    // Update is called once per frame
    void Update()
    {
        energy = UIManager.energy;
        if (energy != oldEnergy)
        {
            for (int i = 0; i < torches.Length; i++)
            {
              //  if (energy >= (visionSteps * i) && energy < visionSteps * (1 + i))
               // {
                    LightSwitch(i);
               // }
            }
        }

        oldEnergy = energy;
        


    }

    private void LightSwitch(int target)
    {
        for (int i = 0; i < torches[target].gameObject.transform.childCount; i++)
        {
            Light light = torches[target].gameObject.transform.GetChild(i).transform.GetChild(0).GetComponent<Light>();
            light.intensity = maxBrightness* Mathf.Clamp01((energy - (visionSteps * target)) / visionSteps);
            light.range = maxRange * Mathf.Clamp01((energy-(visionSteps * target))/visionSteps);
        }
    }
}
