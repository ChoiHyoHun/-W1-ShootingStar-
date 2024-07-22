using System.Collections;
using UnityEngine;
using TMPro;

public class MainMenuTextBlink : MonoBehaviour
{
    public TextMeshProUGUI blinkText; // 메인 메뉴의 TextMeshProUGUI 컴포넌트 참조
    public float blinkInterval = 0.5f; // 깜빡임 간격 (초 단위)

    private Coroutine blinkCoroutine; // 깜빡임 코루틴 참조 변수

    void OnEnable()
    {
        if (blinkText != null)
        {
            blinkCoroutine = StartCoroutine(BlinkText());
        }
    }

    IEnumerator BlinkText()
    {
        while (true)
        {
            // 텍스트 숨기기
            Color color = blinkText.color;
            color.a = 0;
            blinkText.color = color;
            yield return new WaitForSeconds(blinkInterval);

            // 텍스트 보이기
            color.a = 1;
            blinkText.color = color;
            yield return new WaitForSeconds(blinkInterval);
        }
    }

    void OnDisable()
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
        }
    }
}
