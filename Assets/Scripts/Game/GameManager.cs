using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : Singleton<GameManager>
{
    public TextMeshProUGUI scoreText;         // UI에 표시할 점수를 보여줄 Text 컴포넌트
    public TextMeshProUGUI bestScoreText;     // UI에 표시할 최고 기록을 보여줄 Text 컴포넌트
    public int score;             // 현재 점수
    private int bestScore;         // 최고 기록

    //추가
    public Image whiteScreenImage;
    public float fadeDuration = 0.8f;
    private AudioSource managerAudio;

    void Start()
    {
        managerAudio = GetComponent<AudioSource>();
        LoadBestScore();           // 최고 기록 불러오기
        UpdateScoreUI();           // UI에 점수 표시 업데이트
        UpdateBestScoreUI();       // UI에 최고 기록 표시 업데이트

        //추가
        SetImageAlpha(0f);
    }

    // 점수를 추가하고 UI 업데이트
    public void AddScore(int points)
    {
        score += points;
        //스코어를 갱신하기 전에, 스코어가 음수까지 떨어지면 0으로 고정
        if (score < 0)
        {
            score = 0;
        }
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
            bestScoreText.text = "Best: " + bestScore.ToString();
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
        managerAudio.PlayOneShot(managerAudio.clip);
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

    public IEnumerator WhiteScreenEffect()
    {
        // 화면이 하얗게 변함
        yield return StartCoroutine(FadeImage(true));

        // 잠깐 멈춤
        yield return new WaitForSeconds(0.1f);

        // 화면이 서서히 원래대로 돌아옴
        yield return StartCoroutine(FadeImage(false));
    }

    IEnumerator FadeImage(bool fadeToWhite)
    {
        float alpha = fadeToWhite ? 1f : 0f;
        float startAlpha = whiteScreenImage.color.a;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;

            float t = elapsedTime / fadeDuration;

            float newAlpha = Mathf.Lerp(startAlpha, alpha, t * (2f - t));
            SetImageAlpha(newAlpha);
            yield return null;
        }

        SetImageAlpha(alpha);
    }

    void SetImageAlpha(float alpha)
    {
        Color color = whiteScreenImage.color;
        color.a = alpha;
        whiteScreenImage.color = color;
    }



}