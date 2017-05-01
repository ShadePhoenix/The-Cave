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

    public GameObject buildCanvas;
    public GameObject gameUI;

    private Health nodeHealth;

    // Use this for initialization
    void Start ()
    {
        Time.timeScale = 1;
        buildCanvas.SetActive(false);
        gameUI.SetActive(true);
        conMat = startingConMat;
        energy = startingEnergy;
        m_Camera = Camera.main;
        UpdateStats();
        goldVal = goldValue;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateStats();
        BuildNodeClick();
    }

    GameObject currentBuildNode;

    void BuildNodeClick()
    {
        if (Input.GetButtonDown("Fire2") && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit hit;
            if (Physics.Raycast(m_Camera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (hit.collider.tag == "BuildNode" && hit.collider.GetComponent<BuildNode>().allowBuild)
                {
                    //print(hit.collider.name);
                    currentBuildNode = hit.collider.gameObject;
                    buildCanvas.transform.position = currentBuildNode.transform.position + new Vector3(0, 2, 0);
                    buildCanvas.SetActive(true);
                }
                else
                {
                    //print(hit.collider.name);
                    buildCanvas.SetActive(false);
                    currentBuildNode = null;
                }
            }
        }
    }

    //Grabs the turret prefab assigned to the button and sets it to a local variable so it can be placed, and turns on build mode
    public void BuildButton(GameObject turretPrefab)
    {
        if (turretPrefab.GetComponent<TurretAI>() != null && conMat >= turretPrefab.GetComponent<TurretAI>().conMatCost && turretPrefab != null && currentBuildNode != null)
        {
            GameObject builtTurret = Instantiate(turretPrefab, currentBuildNode.transform.position, Quaternion.identity, currentBuildNode.transform);
            currentBuildNode.GetComponent<BuildNode>().turret = builtTurret;
            currentBuildNode.GetComponent<BuildNode>().allowBuild = false;
            nodeHealth = currentBuildNode.GetComponent<Health>();
            nodeHealth.currentHealth = nodeHealth.startHealth;            
            conMat -= turretPrefab.GetComponent<TurretAI>().conMatCost;
            currentBuildNode = null;
            buildCanvas.SetActive(false);
        }
        else if(turretPrefab.GetComponent<SlowTower>() != null && conMat >= turretPrefab.GetComponent<SlowTower>().conMatCost && turretPrefab != null && currentBuildNode != null)
        {
            GameObject builtTurret = Instantiate(turretPrefab, currentBuildNode.transform.position, Quaternion.identity, currentBuildNode.transform);
            currentBuildNode.GetComponent<BuildNode>().turret = builtTurret;
            currentBuildNode.GetComponent<BuildNode>().allowBuild = false;
            nodeHealth = currentBuildNode.GetComponent<Health>();
            nodeHealth.currentHealth = nodeHealth.startHealth;
            conMat -= turretPrefab.GetComponent<SlowTower>().conMatCost;
            currentBuildNode = null;
            buildCanvas.SetActive(false);
        }
        else
        {
            buildCanvas.SetActive(false);
            currentBuildNode = null;
        }

    }

    //Updates score and money text boxes
    public void UpdateStats()
    {
        conMatTB.text = conMat.ToString();
        energyTB.text = energy.ToString();
    }
}