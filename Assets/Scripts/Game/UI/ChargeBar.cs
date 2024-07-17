using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeBar : MonoBehaviour
{
    public Slider chargeBarSliderLeft; // SliderLeft UI 컴포넌트 참조
    public Image chargeBarImageLeft;   // SliderLeft 색상을 변경할 Image 컴포넌트 참조
    public Slider chargeBarSliderRight; // SliderRight UI 컴포넌트 참조
    public Image chargeBarImageRight;  // SliderRight 색상을 변경할 Image 컴포넌트 참조
    public int maxGauge = 100;     // 최대 게이지 값
    public float currentGauge;    // 현재 게이지 값 (float으로 변경)
    public float skillUsageRate = 1f; // 스킬 사용 시 게이지 소모 속도 (per second)

    void Start()
    {
        currentGauge = maxGauge; // 현재 게이지를 최대값으로 초기화

        // 슬라이더의 백그라운드 이미지를 투명하게 설정
        SetBackgroundTransparent(chargeBarSliderLeft);
        SetBackgroundTransparent(chargeBarSliderRight);

        if (chargeBarSliderLeft != null && chargeBarSliderRight != null)
        {
            chargeBarSliderLeft.maxValue = maxGauge; // 슬라이더의 최대 값을 설정
            chargeBarSliderRight.maxValue = maxGauge;
            chargeBarSliderLeft.value = currentGauge; // 초기 게이지 값 설정
            chargeBarSliderRight.value = currentGauge;
        }
    }

    // 슬라이더의 백그라운드 이미지를 투명하게 설정하는 함수
    void SetBackgroundTransparent(Slider slider)
    {
        Image backgroundImage = slider.transform.Find("Background").GetComponent<Image>();
        if (backgroundImage != null)
        {
            Color transparentColor = backgroundImage.color;
            transparentColor.a = 0f; // 알파 값을 0으로 설정하여 완전히 투명하게 만듦
            backgroundImage.color = transparentColor;
        }
    }

    // 발판을 부술 때 호출되는 함수
    public void IncreaseGauge()
    {
        if (currentGauge < maxGauge)
        {
            currentGauge++;
            chargeBarSliderLeft.value = currentGauge;
            chargeBarSliderRight.value = currentGauge;
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
        chargeBarSliderLeft.value = currentGauge; // 슬라이더 업데이트
        chargeBarSliderRight.value = currentGauge;
    }

    // 스킬 게이지를 증가시키는 함수
    public void ChargeSkill(float amount)
    {
        currentGauge += amount;
        if (currentGauge > maxGauge)
        {
            currentGauge = maxGauge;
        }
        chargeBarSliderLeft.value = currentGauge; // 슬라이더 업데이트
        chargeBarSliderRight.value = currentGauge;
    }

}
