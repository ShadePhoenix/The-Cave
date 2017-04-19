using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public GameObject player;
    public Vector3 cameraOffset;
    public float smoothSpeed=2;
    private Vector3 velocity = Vector3.zero;
    public float veiwDistance =10;
    private GameController gameController;
    //private Vector3 rotationTarget;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        transform.position = player.transform.position + cameraOffset;
    }

    // Update is called once per frame
    void LateUpdate () {
        if (!gameController.inMenu && Time.timeScale!=0 && player.GetComponent<PlayerController>().enabled==true)
        {
            Vector3 target = player.GetComponent<PlayerController>().enemyTarget + cameraOffset;// not proud of this, but it moves camera slightly ahead of player, to a max distance.
            if (float.IsNaN(target.x))
            {
                return;
            }
            if (target.x > (player.transform.position.x + veiwDistance))
            {
                target.x = player.transform.position.x + veiwDistance;
            }
            if (target.x < (player.transform.position.x - veiwDistance))
            {
                target.x = player.transform.position.x - veiwDistance;
            }
            if (target.z > (player.transform.position.z + veiwDistance))
            {
                target.z = player.transform.position.z + veiwDistance;
            }
            if (target.z < (player.transform.position.z - veiwDistance))
            {
                target.z = player.transform.position.z - veiwDistance;
            }

            transform.position = Vector3.SmoothDamp(transform.position, target,ref velocity, smoothSpeed);
        }
        else transform.position = player.transform.position + cameraOffset;

        /* if (transform.position.z < -10)
         {
             rotationTarget=new Vector3 (85,180,180);
             if (Vector3.Distance(transform.eulerAngles, rotationTarget) > 0.01f)
             {
                 transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, rotationTarget, Time.deltaTime);
             }
             else transform.eulerAngles = rotationTarget;
         }
         else if (transform.position.z > 10)
         {

         }
         else transform.rotation = Quaternion.Euler(95, 180, 180);*/ // Long code section to cause camera to rotate depending on player depth, to prevent line of sight being obstructed by walls.
                                                                     // just made the 'walls' not tall, problem solved.
    }
}
