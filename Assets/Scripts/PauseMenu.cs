using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PauseMenu : MonoBehaviour{
    public static bool isPause = false;
    public Button resumeButton;
    public Button quitButton;

    public GameObject pauseMenuCanvas;

    void Start() { // 메뉴 비활성화
        resumeButton.onClick.AddListener(Resume);
        quitButton.onClick.AddListener(Quit);
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(isPause){
                Resume();
            } else {
                Pause();
            }
        }
    }

    public void Resume(){
        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
        isPause = false;
    }

    public void Pause(){
        pauseMenuCanvas.SetActive(true);
        Time.timeScale = 0f;
        isPause = true;
    }
    public void Quit() {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
        
    }
}