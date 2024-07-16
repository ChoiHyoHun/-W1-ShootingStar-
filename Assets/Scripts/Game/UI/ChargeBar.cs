using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeBar : MonoBehaviour
{
    public Slider chargeBarSlider; // Slider UI 컴포넌트 참조
    public Image chargeBarImage;  // 색상을 변경할 Image 컴포넌트 참조
    public float duration = 1f;    // 무지개 색상이 한 사이클을 도는 데 걸리는 시간
    public int maxGauge = 100;     // 최대 게이지 값
    private float currentGauge;    // 현재 게이지 값 (float으로 변경)
    public float skillUsageRate = 1f; // 스킬 사용 시 게이지 소모 속도 (per second)

    void Start()
    {
        currentGauge = maxGauge; // 현재 게이지를 최대값으로 초기화

        if (chargeBarImage != null)
        {
            StartCoroutine(RainbowEffect());
        }

        if (chargeBarSlider != null)
        {
            chargeBarSlider.maxValue = maxGauge; // 슬라이더의 최대 값을 설정
            chargeBarSlider.value = currentGauge; // 초기 게이지 값 설정
        }
    }

    // 발판을 부술 때 호출되는 함수
    public void IncreaseGauge()
    {
        if (currentGauge < maxGauge)
        {
            currentGauge++;
            chargeBarSlider.value = currentGauge;
        }
    }

    // 스킬을 사용하는 함수
    public void UseSkill()
    {
        // 게이지를 감소시킴
        currentGauge -= skillUsageRate * Time.deltaTime;
        if (currentGauge < 0)
        {
            currentGauge = 0;
        }
        chargeBarSlider.value = currentGauge; // 슬라이더 업데이트
    }

    // 스킬 게이지를 증가시키는 함수
    public void ChargeSkill(float amount)
    {
        currentGauge += amount;
        if (currentGauge > maxGauge)
        {
            currentGauge = maxGauge;
        }
        chargeBarSlider.value = currentGauge; // 슬라이더 업데이트
    }


    IEnumerator RainbowEffect()
    {
        float elapsedTime = 0f;
        while (true)
        {
            elapsedTime += Time.deltaTime;
            float hue = Mathf.Repeat(elapsedTime / duration, 1f); // 0에서 1 사이의 Hue 값 반복
            chargeBarImage.color = Color.HSVToRGB(hue, 1f, 1f); // Saturation과 Value는 1로 고정
            yield return null; // 한 프레임 대기
        }
    }
}
