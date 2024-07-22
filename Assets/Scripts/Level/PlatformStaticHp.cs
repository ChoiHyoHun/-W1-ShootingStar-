using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlatformStaticHp : MonoBehaviour
{
    [SerializeField]
    private int hp;

    public PopUpScore scoreTextPfb;
    private PopUpScore scoreText;

    //추가
    [SerializeField]
    private float rayDistance = 3f;
    private bool wasHitLastFrame = false;
    float distanceLastFrame;

    private bool isAvoided = false;
    Vector3 raySpawnOffset = new Vector2(0f, 0.5f);

    void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position + raySpawnOffset, Vector2.up, rayDistance, LayerMask.GetMask("PlayerWithPlatform"));


        if (hit.rigidbody != null)
        {
            if (!wasHitLastFrame)
            {
                //계속 갱신되고 있으니까 => 내 위에 Player 계속 있다.
                wasHitLastFrame = true;
                distanceLastFrame = hit.distance;
            }

        }
        else //ray에 걸리는거 사라짐. 
        {
            if (wasHitLastFrame && !isAvoided) //근데 아까까지 위에 Player가 있었음
            {
                int avoidScore = CalculateAvoidScore(distanceLastFrame);

                isAvoided = true; //있다가 갔으니까 피한거지.

                PlayerController.Instance.chargeBar.ChargeSkill(3f);
                GameManager.Instance.AddScore(avoidScore);
                //점수 텍스트 팝업
                scoreText = Instantiate(scoreTextPfb, PlayerController.Instance.transform.position, Quaternion.identity);
                scoreText.SettingText(CalculateAvoidScore(distanceLastFrame));
            }

        }

    }

    private int CalculateAvoidScore(float distance)
    {
        // 거리에 따라 점수를 설정합니다.
        if (distance <= 1f && distance > 0f)
        {
            return 500;
        }
        else if (distance <= 2f && distance > 1f)
        {
            return 400;
        }
        else if (distance <= 3f && distance > 2f)
        {
            return 300;
        }
        else if (distance <= 5f && distance > 3f)
        {
            return 100;
        }
        else
        {
            return 0; // 지정된 거리 범위 외에는 점수를 부여하지 않습니다.
        }
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (collisionInfo.collider.tag == "Player")
        {
            //파괴 가능한가?
            if (CanBreak())
            {
                Break();
                //대쉬 상태가 아닌가?
                if (!PlayerController.Instance.isDash)
                    //바운스 호출
                    PlayerController.Instance.Bounce(transform.position.y);

                //Break();
            }
            else
            {
                GameManager.Instance.FailGame();
            }

        }
    }

    //플랫폼 파괴 메서드
    public void Break()
    {
        //점수 계산
        GameManager.Instance.AddScore(CalculateScore());

        //점수 텍스트 팝업
        scoreText = Instantiate(scoreTextPfb, transform.position, Quaternion.identity);
        scoreText.SettingText(CalculateScore());

        //플랫폼 끄기
        gameObject.SetActive(false);
    }

    //피버 대쉬 중이 아닐 때 플랫폼을 밟으면 점수를 까도록 수정
    public int CalculateScore()
    {
        int basicPoint = 100;
        if (PlayerController.Instance.isDash)
        {
            return basicPoint * (1 + 2 * hp) / 5;
        }
        else
        {
            return -basicPoint * (1 + 2 * hp);
        }

    }


    private bool CanBreak()
    {
        if (PlayerController.Instance.ACCStep >= hp || PlayerController.Instance.isDash)
        {
            return true;
        }
        else
        {
            return false;
        }

    }


}
