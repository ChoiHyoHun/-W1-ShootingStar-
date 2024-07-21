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



    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (collisionInfo.collider.tag == "Player")
        {
            //파괴 가능한가?
            if (CanBreak())
            {
                //대쉬 상태가 아닌가?
                if (!PlayerController.Instance.isDash)
                    //바운스 호출
                    PlayerController.Instance.Bounce(transform.position.y);

                Break();
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
        //점수 올리기
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
            return basicPoint * (1 + 2 * hp);
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
