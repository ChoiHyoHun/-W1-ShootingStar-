using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public Text scoreText;         // UI에 표시할 점수를 보여줄 Text 컴포넌트
    public Text bestScoreText;     // UI에 표시할 최고 기록을 보여줄 Text 컴포넌트
    private int score;             // 현재 점수
    private int bestScore;         // 최고 기록



    void Start()
    {
        LoadBestScore();           // 최고 기록 불러오기
        UpdateScoreUI();           // UI에 점수 표시 업데이트
        UpdateBestScoreUI();       // UI에 최고 기록 표시 업데이트
    }


    // 점수를 추가하고 UI 업데이트
    public void AddScore(int points)
    {
        score += points;
        UpdateScoreUI();

        // 최고 기록 갱신
        if (score > bestScore)
        {
            bestScore = score;
            SaveBestScore(); // 최고 기록 저장
            UpdateBestScoreUI(); // UI에 최고 기록 표시 업데이트
        }
    }

    // UI에 점수 표시 업데이트
    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }

    // UI에 최고 기록 표시 업데이트
    private void UpdateBestScoreUI()
    {
        if (bestScoreText != null)
        {
            bestScoreText.text = "Best Score: " + bestScore.ToString();
        }
    }

    // 최고 기록 저장
    private void SaveBestScore()
    {
        PlayerPrefs.SetInt("BestScore", bestScore);
        PlayerPrefs.Save();
    }

    // 최고 기록 불러오기
    private void LoadBestScore()
    {
        bestScore = PlayerPrefs.GetInt("BestScore", 0);
    }

    // 게임이 실패했을 때 호출되는 메서드
    public void FailGame()
    {
        if (PlayerController.Instance != null)
        {
            // 플레이어 캐릭터 파괴
            Destroy(PlayerController.Instance.gameObject);
        }

        StartCoroutine(WaitForDeath());
    }

    // 일정 시간 대기 후 씬을 다시 로드하는 코루틴
    IEnumerator WaitForDeath()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 현재 씬 다시 로드
    }
}