using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNight : MonoBehaviour {
    public float nightLength;
    public float dayLength;
    public static float nightNumber;
    public bool isNight = false;
    private float dayRotate = 0;
    private float nightTimer = 0;
    [Tooltip("The Total Arc the sun follows during the day")]
    public float dayAngle = 240;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (isNight)
        {
            nightTimer += Time.deltaTime;
            Debug.Log(nightTimer);
            if (nightTimer > nightLength)
            {
                isNight = false;
                nightTimer = 0;
                transform.Rotate(Vector3.left * dayAngle);
            }
        }
        if (!isNight)
        {
            transform.Rotate(Vector3.right * Time.deltaTime * dayAngle/dayLength);
            dayRotate += Time.deltaTime * dayAngle / dayLength;
            if (dayRotate > dayAngle)
            {
                dayRotate = 0;
                nightNumber += 1;
                isNight = true;
            }
        }

	}
}
