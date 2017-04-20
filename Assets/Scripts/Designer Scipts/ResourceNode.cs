using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNode : MonoBehaviour {
    public bool energy=true;
    public float generationRate;
    public float maxResources;
    public float maxBrightness;
    public GameObject[] lightSource;
    public float currentResources;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (currentResources <= maxResources)
        {
            currentResources += generationRate * Time.deltaTime;
        }
        for (int i =0;i<lightSource.Length; i++)
        {
            Light light = lightSource[i].GetComponent<Light>();
            light.intensity = maxBrightness * (currentResources / maxResources);
            light.range = maxBrightness * (currentResources / maxResources) * 10;
        }
	}
}
