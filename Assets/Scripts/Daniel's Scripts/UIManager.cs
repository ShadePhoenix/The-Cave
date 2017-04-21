using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Camera m_Camera;

    public Text conMatTB;
    static public int conMat;
    public int startingConMat;

    public Text energyTB;
    static public int energy;
    public int startingEnergy;

    static public int gold = 0;
    [Tooltip("How much gold is worth.")]
    public int goldValue;
    static public int goldVal;

    //public GameObject cursorObject;

    public GameObject buildPanel;
    public GameObject gameUI;

    public Sprite targetSprite;

    // Use this for initialization
    void Start ()
    {
        Time.timeScale = 1;
        buildPanel.SetActive(false);
        //cursorObject.SetActive(false);
        gameUI.SetActive(true);
        conMat = startingConMat;
        energy = startingEnergy;
        //m_Camera = Camera.main;
        UpdateStats();
        goldVal = goldValue;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateStats();
        if (!CastleController.playerActive)
        {
            //cursorObject.SetActive(true);
            //TargetCursor();
        }
        BuildNodeClick();
    }


    //void TargetCursor()
    //{
    //    Vector3 mousePos = Input.mousePosition;
    //    mousePos.z = 950f;
    //    cursorObject.transform.position = m_Camera.ScreenToWorldPoint(mousePos);
    //}

    GameObject currentBuildNode;

    void BuildNodeClick()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit point;
            if (Physics.Raycast(m_Camera.ScreenPointToRay(Input.mousePosition), out point))
            {
                if (point.collider.tag == "BuildNode" && point.collider.GetComponent<BuildNode>().allowBuild)
                {
                    currentBuildNode = point.collider.gameObject;
                    buildPanel.SetActive(true);
                    buildPanel.transform.position = currentBuildNode.transform.position;
                }
            }
        }
    }

    //Grabs the turret prefab assigned to the button and sets it to a local variable so it can be placed, and turns on build mode
    public void BuildButton(GameObject turretPrefab)
    {
        if (conMat >= turretPrefab.GetComponent<TurretAI>().conMatCost)
        {
            GameObject builtTurret = Instantiate(turretPrefab, currentBuildNode.transform.position, Quaternion.identity);
            currentBuildNode.GetComponent<BuildNode>().turret = builtTurret;
            currentBuildNode.GetComponent<BuildNode>().allowBuild = false;
            conMat -= turretPrefab.GetComponent<TurretAI>().conMatCost;
            currentBuildNode = null;
            buildPanel.SetActive(false);
        }
    }

    

    //Updates score and money text boxes
    public void UpdateStats()
    {
        conMatTB.text = conMat.ToString();
        energyTB.text = energy.ToString();
    }
}