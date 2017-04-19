using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour {
    public float score = 0;
    private Slider foodSlider;
    public bool inMenu = false;
    private GameObject gameMenu;
    private bool spawning;
    private GameObject spawnMenu;
    private GameObject player;
    public GameObject[] entrances;
    public int currentEntrance;
    public float countDownMax = 300.0f;
    public Text countDown;
    public float oneStar , twoStar , threeStar;
    public Text endText;
    public string levelNumber;
    private float gameOverTimer=0;
    private bool gameOverHappening;
    private string failCause;
	// Use this for initialization
	void Start () {
        Time.timeScale = 1;
        foodSlider = GameObject.Find("FoodSlider").GetComponent<Slider>();
        foodSlider.maxValue = threeStar;
        gameMenu = GameObject.Find("GameMenu");
        gameMenu.SetActive(false);
        spawnMenu = GameObject.Find("SpawnMenu");
        spawnMenu.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player");
        countDown = GameObject.Find("Timer").GetComponent<Text>();
        endText = GameObject.Find("EndText").GetComponent<Text>();
        player.GetComponent<PlayerController>().enabled = true;
        //GameObject[] sortedEntrances= entrances.OrderBy(Vector2=> v.x)).ToArray<GameObject>();
        //  System.Array.Sort(entrances);
    }
	
	// Update is called once per frame
	void Update () {
        countDownMax -= Time.deltaTime;
        foodSlider.value = score;
        if (score >= threeStar)
        {
            GameObject.Find("ThreeStar").GetComponent<Image>().color = Color.white;
        }
        if (score>= twoStar)
        {
            GameObject.Find("TwoStar").GetComponent<Image>().color = Color.white;
        }
        if (score >= oneStar)
        {
            GameObject.Find("OneStar").GetComponent<Image>().color = Color.white;
        }
        if (Input.GetKeyDown(KeyCode.Escape) && inMenu)
        {
            Resume();

        }
        else if (Input.GetKeyDown(KeyCode.Escape) && !inMenu)
        {
            Time.timeScale = 0;
            inMenu = true;
            gameMenu.SetActive(true);
        }
        if (countDownMax < 60.0f)
        {
            countDown.color = Color.red;
            countDown.fontSize = 30;
        }
        if (countDownMax < 0.0f)
        {
            score -= (Time.deltaTime * 100);

            if (score < 0)
            {
                GameOver("TimeOut");
            }
        }
        countDown.text ="" + Mathf.Round(countDownMax);
        if (gameOverHappening)
        {
            gameOverTimer += Time.deltaTime;
        }
        if (gameOverTimer > 2)
        {
            GameObject.Find("BlackGround").GetComponent<Image>().enabled = true;
            Time.timeScale = 0;
            inMenu = true;
            gameMenu.SetActive(true);
            Button resumeButton = GameObject.Find("Resume").GetComponent<Button>();
            //GameObject.Find("Resume").GetComponent<Button>().interactable = false;
            if (resumeButton != null)
            {
                resumeButton.interactable = false;
            }
            endText.text = "Kg Eaten:" + (score / 1000).ToString("F2") + "\n" + failCause;
        }

    }
    public void Resume()
    {
        Time.timeScale = 1;
        inMenu = false;
        gameMenu.SetActive(false);
    }
    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);


    }
    public void MainMenu(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void SpawnMenu(bool onoff)
    {
        spawnMenu.SetActive(onoff);
        if (score>oneStar && onoff == true)
        {

        }
    }
    public void WhichEntrance()
    {
        for (int i=0; i<entrances.Length; i++)
        {
            if (entrances[i].transform.position.x == player.transform.position.x)
            {
                currentEntrance = i;
            }
        }
    }

    public void GameOver(string fail)
    {
        Debug.Log(fail);
        failCause = fail;
        gameOverHappening = true;
    }
    public void FinishLevel()
    {
        GameObject.Find("BlackGround").GetComponent<Image>().enabled=true;
        Time.timeScale = 0;
        inMenu = true;
        gameMenu.SetActive(true);
        GameObject.Find("Resume").GetComponent<Button>().interactable = false;
        PlayerPrefs.SetInt(levelNumber+"Complete", 1);
        
        int stars = 0;
        if (score >= threeStar)
        {
            stars = 3;
        }
        else if (score >= twoStar)
        {
            stars = 2;
        }
        else stars = 1;
        if (PlayerPrefs.GetInt(levelNumber + "Rating", stars) < stars)
        {
            PlayerPrefs.SetInt(levelNumber + "Rating", stars);
        }
        endText.text = "Kg Eaten:" + (score / 1000).ToString("F2") + "\nStars Earnt:"+stars;

    }
}
