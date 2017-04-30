using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceNode : MonoBehaviour {
    public bool energy=true;
    public float generationRate;
    public float maxResources;
    public float maxBrightness;
    public int gatherRate;
    private float timer;
    private float timerTarget;
    public GameObject[] lightSource;
    public float currentResources;
    public GameObject worldText;
    private Text output;
	// Use this for initialization
	void Start () {
        timerTarget = timer + 1;
        output = worldText.GetComponent<Text>();
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
        output.text = "" + Mathf.FloorToInt(currentResources);
	}
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" )
        {
            timer += Time.deltaTime;
            if (timer >= timerTarget && currentResources > gatherRate)
            {
                currentResources -= gatherRate;
                if (energy)
                {
                    UIManager.energy += gatherRate;
                }
                if (!energy)
                {
                    UIManager.conMat += gatherRate;
                }
                timerTarget += 1;
            }
            else if (timer >= timerTarget && currentResources < gatherRate)
            {
                int roundDown = Mathf.FloorToInt(currentResources);
                currentResources -= roundDown;
                if (energy)
                {
                    UIManager.energy += roundDown;
                }
                if (!energy)
                {
                    UIManager.conMat += roundDown;
                }
                timerTarget += 1;
            }

        }
    }
}
