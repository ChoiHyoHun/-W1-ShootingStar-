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
            if (CanBreak())
            {
                if (!PlayerController.Instance.isDash)
                    PlayerController.Instance.Bounce(transform.position.y);
                GameManager.Instance.AddScore(CalculateScore());

                scoreText = Instantiate(scoreTextPfb, transform.position, Quaternion.identity);
                scoreText.SettingText(CalculateScore());


                gameObject.SetActive(false);
            }
            else
            {
                GameManager.Instance.FailGame();
            }

        }
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
