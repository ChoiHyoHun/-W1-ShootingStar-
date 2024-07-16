using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public string SceneToLoad;
    public Button StartButton;
    public GameObject MainMenuCanvas;
    public bool isGameStart = false;

    void Start()
    {
        Time.timeScale = 0f;
        StartButton.onClick.AddListener(GameStart);
        if (isGameStart == false)
        {
            MainMenuCanvas.SetActive(true);
        }
    }


    void Update()
    {

    }

    public void GameStart()
    {
        MainMenuCanvas.SetActive(false);
        isGameStart = true;
        Time.timeScale = 1f;
    }

}