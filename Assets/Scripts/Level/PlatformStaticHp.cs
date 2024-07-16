using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformStaticHp : MonoBehaviour
{
    [SerializeField]
    private int hp;

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (collisionInfo.collider.tag == "Player")
        {
            if (CanBreak())
            {
                if (!PlayerController.Instance.isDash)
                    PlayerController.Instance.Bounce();
                gameObject.SetActive(false);
            }
            else
            {
                // 임시로 죽으면 게임 멈춤
                GameManager.Instance.FailGame();
                Debug.Log("YOU DIED");
            }

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
