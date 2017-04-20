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
    Camera m_Camera;

    public Text conMatTB;
    static public int conMat;
    public int startingConMat;

    public Text energyTB;
    static public int energy;
    public int startingEnergy;
   
    static public int score;

    //public GameObject cursorObject;

    public GameObject buildPanel;
    public GameObject gameUI;
    //public GameObject pauseMenu;
    //public GameObject gameOverMenu;
    //public GameObject gameWonMenu;

    public bool gameOver = false;
    public bool gameWon = false;

    //public Sprite targetSprite;
    //public Sprite buildSprite;

    //public GameObject[] turretPrefabs;
    //public GameObject[] enemyPrefabs;
    //public GameObject buttonPrefab;
    //public GameObject[] buttonPositions; 

    //If this is true, we're in "build mode" and the next click will place a building
    static public bool isBuilding = false;
    static public bool isRemoving = false;
    static public bool uiMode = false;

    enum UIState {Pause, Build, Repair, Remove, EndGame}
    UIState uiState;

    // Use this for initialization
    void Start ()
    {
        Time.timeScale = 1;
        isBuilding = false;
        isRemoving = false;
        uiMode = false;
        score = 0;
        gameUI.SetActive(true);
        buildPanel.SetActive(false);
        //gameOverMenu.SetActive(false);
        //pauseMenu.SetActive(false);
        //gameWonMenu.SetActive(false);
        conMat = startingConMat;
        energy = startingEnergy;
        m_Camera = Camera.main;
        UpdateStats();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateStats();
        //Construction();
        //Enables uiMode if any of these are true
        //if (isRemoving || isBuilding || isPaused || gameOver)
        //    uiMode = true;
        //else
        //    uiMode = false;
        //if (Input.GetKeyDown(KeyCode.Escape))
        //MenuPR(!isPaused);
        //TargetCursor();
        BuildNodeClick();
    }


    //void TargetCursor()
    //{
    //    Vector3 mousePos = Input.mousePosition;
    //    mousePos.z = 950f;
    //    cursorObject.transform.position = m_Camera.ScreenToWorldPoint(mousePos);
    //    if (isBuilding || isRemoving)
    //        cursorObject.GetComponentInChildren<SpriteRenderer>().sprite = buildSprite;
    //    else
    //        cursorObject.GetComponentInChildren<SpriteRenderer>().sprite = targetSprite;
    //}

    GameObject currentBuildNode;
    Transform buildPanelPos;

    void BuildNodeClick()
    {
        if (Input.GetButtonDown("Fire1") && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit hit;
            if (Physics.Raycast(m_Camera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                if (hit.collider.tag == "BuildNode")
                {
                    currentBuildNode = hit.collider.gameObject;
                    buildPanelPos = hit.collider.transform;
                    buildPanel.SetActive(true);
                    buildPanel.transform.position = m_Camera.WorldToScreenPoint(m_Camera.ScreenToWorldPoint(Input.mousePosition));
                    uiState = UIState.Build;
                }
            }
        }
    }

    //Grabs the turret prefab assigned to the button and sets it to a local variable so it can be placed, and turns on build mode
    public void BuildButton(GameObject turretPrefab)
    {
        if (conMat >= turretPrefab.GetComponent<TurretAI>().conMatCost)
        {
            buildPanel.SetActive(false);
            Instantiate(turretPrefab, buildPanelPos.position, Quaternion.identity);
            conMat -= turretPrefab.GetComponent<TurretAI>().conMatCost;
            currentBuildNode = null;
        }
    }

    //Activates build mode
    //public void RemoveButton()
    //{
    //    isRemoving = true;
    //    isBuilding = !isRemoving;
    //}

    //void Construction()
    //{
    //    //This is for building turrets
    //    if (isBuilding && turretPrefab != null)
    //    {
    //        if (Input.GetButtonDown("Fire1") && !EventSystem.current.IsPointerOverGameObject() && money >= turretPrefab.GetComponent<TurretAI>().cost)
    //        {
    //            RaycastHit hit;
    //            if (Physics.Raycast(m_Camera.ScreenPointToRay(Input.mousePosition), out hit))
    //            {
    //                if (hit.collider.tag == "BuildPos")
    //                {
    //                    Instantiate(turretPrefab, hit.collider.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
    //                    money -= turretPrefab.GetComponent<TurretAI>().cost;
    //                    UpdateStats();
    //                }
    //                turretPrefab = null;
    //                StartCoroutine(WaitTimer());
    //            }
    //        }
    //        else if (Input.GetButtonDown("Fire1") && money < turretPrefab.GetComponent<TurretAI>().cost)
    //            StartCoroutine(WaitTimer());
    //    }
    //    //This is for removing turrets
    //    if (isRemoving)
    //    {
    //        if (Input.GetButtonDown("Fire1") && !EventSystem.current.IsPointerOverGameObject())
    //        {
    //            RaycastHit hit;
    //            if (Physics.Raycast(m_Camera.ScreenPointToRay(Input.mousePosition), out hit))
    //            {
    //                if (hit.collider.tag == "Turret")
    //                {
    //                    money += hit.collider.GetComponent<TurretAI>().cost / 2;
    //                    UpdateStats();
    //                    Destroy(hit.collider.gameObject);
    //                }
    //                StartCoroutine(WaitTimer());
    //            }
    //        }
    //    }
    //}

    //Updates score and money text boxes
    public void UpdateStats()
    {
        conMatTB.text = conMat.ToString();
        energyTB.text = energy.ToString();
    }

    //Handles the pause menus and pauses the game
    //bool isPaused = false;
    //public void MenuPR(bool pause = false)
    //{
    //    //Pause
    //    isPaused = pause;
    //    if (pause && !gameOver && !gameWon)
    //    {
    //        gameUI.SetActive(!pause);
    //        pauseMenu.SetActive(pause);
    //        Time.timeScale = 0;
    //    }
    //    //Unpause if Paused
    //    else if (!pause && !gameOver && !gameWon)
    //    {
    //        gameUI.SetActive(!pause);
    //        pauseMenu.SetActive(pause);
    //        Time.timeScale = 1;
    //    }
    //    if (pause && gameOver && !gameWon)
    //    {
    //        gameOverMenu.SetActive(gameOver);
    //        gameUI.SetActive(!gameOver);
    //        Time.timeScale = 0;
    //    }
    //    if (pause && !gameOver && gameWon)
    //    {
    //        gameWonMenu.SetActive(gameWon);
    //        gameUI.SetActive(!gameWon);
    //        Time.timeScale = 0;
    //    }
    //}

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //Handles the gameover menu
    //public void GameOver()
    //{
    //    gameOver = true;
    //    MenuPR(gameOver);
    //}

    //public void GameWon()
    //{
    //    gameWon = true;
    //    MenuPR(gameWon);
    //}

    //Takes you back to the main menu
    public void Quit()
    {
        SceneManager.LoadScene("Main Menu");
    }

    //This does something that helps, I swear
    IEnumerator WaitTimer()
    {
        yield return new WaitForSeconds(0.2f);
        uiMode = false;
        if (isBuilding)
            isBuilding = !isBuilding;
        else if (isRemoving)
            isRemoving = !isRemoving;
    }
}