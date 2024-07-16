using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManu : MonoBehaviour
{
    
    public string SceneToLoad;
    public Button RestartButton;
    public GameObject GameOverMenuCanvas;
    public bool isGameOver=false;

    void Start() { 
        RestartButton.onClick.AddListener(ReStart);
    }

   
    void Update()
    {
        if(isGameOver==true){
             GameOverMenuCanvas.SetActive(true);
        }
    }

    public void ReStart(){
        SceneManager.LoadScene(SceneToLoad);
        GameOverMenuCanvas.SetActive(false);
    }

}