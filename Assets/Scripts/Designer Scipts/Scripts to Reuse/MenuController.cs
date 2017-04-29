using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {
    // Use this for initialization
    public GameObject menus;
    private bool inMenu = false;
	void Start () {
        menus = GameObject.Find("InGameMenu");
        if (menus!=null)
        {
            menus.SetActive(false);
        }
	}

    // Update is called once per frame
    void Update()
    {
        if (menus != null)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && inMenu)
            {
                Resume();
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && !inMenu)
            {
                Time.timeScale = 0;
                inMenu = true;
                menus.SetActive(true);
            }
        }
    }
    public void SceneChange(string sceneName)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("GameLevel Stu");
    }

    public void CloseProgram()
    {
        Application.Quit();
    }
    public void Resume()
    {
        Time.timeScale = 1;
        inMenu = false;
        menus.SetActive(false);

    }
}
