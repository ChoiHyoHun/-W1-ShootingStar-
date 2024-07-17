
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Button startButton;
    private bool startGame = false;
    void Start()
    {
        if (startButton != null)
        {
            startButton.onClick.AddListener(StartGame);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && startGame == false)
        {
            StartGame();
        }
    }

    void StartGame()
    {
        SceneManager.LoadScene("GameScene");
        startGame = true;
    }
}