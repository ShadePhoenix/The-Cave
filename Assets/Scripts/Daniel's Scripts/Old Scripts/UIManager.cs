using System.IO;
using System.Xml;
using System.Linq;
using System.Collections;
using System.Xml.Serialization;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    //List<HighScore> highScores;
    //static public string filePath;

    Camera m_Camera;

    public Text moneyTB;
    static public int money;
    public int startingMoney;

    public Text scoreTB;    
    static public int score;

    public Text endScore;
    public Text winScore;

    public GameObject cursorObject;

    public GameObject gameUI;
    public GameObject pauseMenu;
    public GameObject gameOverMenu;
    public GameObject gameWonMenu;

    public bool gameOver = false;
    public bool gameWon = false;

//    GameObject enemySpawner;
    public Sprite targetSprite;
    public Sprite buildSprite;

    public GameObject[] turretPrefabs;
    public GameObject[] enemyPrefabs;
    public GameObject buttonPrefab;
    public GameObject[] buttonPositions;

    GameObject turretPrefab; 

    //If this is true, we're in "build mode" and the next click will place a building
    static public bool isBuilding = false;
    static public bool isRemoving = false;
    static public bool uiMode = false;

    void Awake()
    {
//        enemySpawner = GameObject.FindGameObjectWithTag("Spawner");
//        enemySpawner.GetComponent<EnemySpawner>().enemyPrefab = enemyPrefabs;

    }

    // Use this for initialization
    void Start ()
    {
        isBuilding = false;
        isRemoving = false;
        uiMode = false;
        score = 0;
        scoreTB.text = score.ToString();
        Time.timeScale = 1;
        //filePath = PlayerPrefs.GetString("FilePath");
        gameUI.SetActive(true);
        gameOverMenu.SetActive(false);
        pauseMenu.SetActive(false);
        gameWonMenu.SetActive(false);
        money = startingMoney;
        m_Camera = Camera.main;
        UpdateStats();
        ButtonIni();
    }

    //Spawns buttons in the menu for each prefab defined
    void ButtonIni()
    {
        buttonPositions = GameObject.FindGameObjectsWithTag("ButtonPos").OrderBy(go => go.name).ToArray();
        int num = 0;
        if (buttonPositions.Length > 0)
        {
            foreach (GameObject prefab in turretPrefabs)
            {
                GameObject buildButton = Instantiate(buttonPrefab, buttonPositions[num].transform.position, Quaternion.identity, buttonPositions[num].transform);
                buildButton.GetComponent<BuildButton>().turretPrefab = prefab.GetComponent<TurretAI>().gameObject;
                num++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Construction();
        //Enables uiMode if any of these are true
        if (isRemoving || isBuilding || isPaused || gameOver)
            uiMode = true;
        else
            uiMode = false;
        if (Input.GetKeyDown(KeyCode.Escape))
            MenuPR(!isPaused);
        TargetCursor();
    }

    
    void TargetCursor()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 950f;
        cursorObject.transform.position = m_Camera.ScreenToWorldPoint(mousePos);
        if (isBuilding || isRemoving)
            cursorObject.GetComponentInChildren<SpriteRenderer>().sprite = buildSprite;
        else
            cursorObject.GetComponentInChildren<SpriteRenderer>().sprite = targetSprite;
    }

    //Grabs the turret prefab assigned to the button and sets it to a local variable so it can be placed, and turns on build mode
    public void BuildButton(GameObject turret)
    {
        if (turret != null)
        {
            isBuilding = true;
            isRemoving = !isBuilding;
            turretPrefab = turret;
        }
    }

    //Activates build mode
    public void RemoveButton()
    {
        isRemoving = true;
        isBuilding = !isRemoving;
    }

    void Construction()
    {
        //This is for building turrets
        if (isBuilding && turretPrefab != null)
        {
            if (Input.GetButtonDown("Fire1") && !EventSystem.current.IsPointerOverGameObject() && money >= turretPrefab.GetComponent<TurretAI>().cost)
            {
                RaycastHit hit;
                if (Physics.Raycast(m_Camera.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    if (hit.collider.tag == "BuildPos")
                    {
                        Instantiate(turretPrefab, hit.collider.transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
                        money -= turretPrefab.GetComponent<TurretAI>().cost;
                        UpdateStats();
                    }
                    turretPrefab = null;
                    StartCoroutine(WaitTimer());
                }
            }
            else if (Input.GetButtonDown("Fire1") && money < turretPrefab.GetComponent<TurretAI>().cost)
                StartCoroutine(WaitTimer());
        }
        //This is for removing turrets
        if (isRemoving)
        {
            if (Input.GetButtonDown("Fire1") && !EventSystem.current.IsPointerOverGameObject())
            {
                RaycastHit hit;
                if (Physics.Raycast(m_Camera.ScreenPointToRay(Input.mousePosition), out hit))
                {
                    if (hit.collider.tag == "Turret")
                    {
                        money += hit.collider.GetComponent<TurretAI>().cost / 2;
                        UpdateStats();
                        Destroy(hit.collider.gameObject);
                    }
                    StartCoroutine(WaitTimer());
                }
            }
        }
    }

    //Updates score and money text boxes
    public void UpdateStats()
    {
        scoreTB.text = "Score: " + score;
        moneyTB.text = "Money: $" + money;
    }

    //Handles the pause menus and pauses the game
    bool isPaused = false;
    public void MenuPR(bool pause = false)
    {
        //Pause
        isPaused = pause;
        if (pause && !gameOver && !gameWon)
        {
            gameUI.SetActive(!pause);
            pauseMenu.SetActive(pause);
            Time.timeScale = 0;
        }
        //Unpause if Paused
        else if (!pause && !gameOver && !gameWon)
        {
            gameUI.SetActive(!pause);
            pauseMenu.SetActive(pause);
            Time.timeScale = 1;
        }
        if (pause && gameOver && !gameWon)
        {
            gameOverMenu.SetActive(gameOver);
            gameUI.SetActive(!gameOver);
            Time.timeScale = 0;
        }
        if (pause && !gameOver && gameWon)
        {
            gameWonMenu.SetActive(gameWon);
            gameUI.SetActive(!gameWon);
            Time.timeScale = 0;
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //Handles the gameover menu
    public void GameOver()
    {
        gameOver = true;
        MenuPR(gameOver);
        endScore.text = "You have been defeated! \n You were able to score: " + score + "\n Defeating " + /*EnemySpawner.defeatedEnemies + " out of " + EnemySpawner.spawnEnemies +*/ "\n Maybe next time you'll have better luck!";
    }

    public void GameWon()
    {
        gameWon = true;
        MenuPR(gameWon);
        winScore.text = "Congratulations! You've defeated all enemies. \n With a score of " + score;
    }

    //Takes you back to the main menu
    public void Quit()
    {
        SceneManager.LoadScene("Main Menu");
    }

    //void GetHighScore()
    //{
    //    StreamReader reader = new StreamReader(filePath);
    //    string[] temp = reader.ReadToEnd().Split(',');
    //    print(temp[0]);
    //    print(temp[1]);
    //    print(temp[2]);
    //}

    //void SaveHighScore()
    //{

    //}

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

//public struct HighScore
//{
//    public int rank;
//    public string playerName;
//    public int playerScore;
//    public HighScore(int _rank, string _playerName, int _playerScore)
//    {
//        rank = _rank;
//        playerName = _playerName;
//        playerScore = _playerScore;
//    }
//}