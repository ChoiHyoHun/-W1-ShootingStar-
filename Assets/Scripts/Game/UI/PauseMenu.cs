using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPause = false;
    public Button resumeButton;
    public Button quitButton;

    public Button restartButton;

    public GameObject pauseMenuCanvas;

    void Start()
    { // 메뉴 비활성화
        resumeButton.onClick.AddListener(Resume);
        quitButton.onClick.AddListener(Quit);
        // restartButton.onClick.AddListener(Restart);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }


        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            HardReset();
        }
    }

    public void Resume()
    {
        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
        isPause = false;
    }

    public void Pause()
    {
        pauseMenuCanvas.SetActive(true);
        Time.timeScale = 0f;
        isPause = true;
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void Restart()
    {
        pauseMenuCanvas.SetActive(false);
        isPause = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //최고 기록 임의로 삭제하는 버튼
    private void HardReset()
    {
        PlayerPrefs.DeleteKey("BestScore");
    }

}