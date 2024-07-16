using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlatformStaticHp : MonoBehaviour
{
    [SerializeField]
    private int hp;

    /*
    [SerializeField]
    TextMeshProUGUI hpTextPfb;
    TextMeshProUGUI hpText;
    Canvas fixedCanvas;
    void Awake()
    {
        fixedCanvas = GameObject.Find("FixedCanvas").GetComponent<Canvas>();
        hpText = Instantiate(hpTextPfb, transform.position, Quaternion.identity, fixedCanvas.GetComponent<Canvas>().transform);
    }
    */

    void Start()
    {
        //hpText.SetText(hp.ToString());
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (collisionInfo.collider.tag == "Player")
        {
            if (CanBreak())
            {
                if (!PlayerController.Instance.isDash)
                    PlayerController.Instance.Bounce();
                GameManager.Instance.AddScore(CalculateScore());
                gameObject.SetActive(false);
                //Destroy(hpText);
            }
            else
            {
                // 임시로 죽으면 게임 멈춤
                GameManager.Instance.FailGame();
                Debug.Log("YOU DIED");
            }

        }
    }

    private int CalculateScore()
    {
        int basicPoint = 100;
        return basicPoint * (1 + hp);

    }

    /*
    void OnDisable()
    {
        if (hpText != null)
        {
            Destroy(hpText.gameObject);
        }
    }
    */

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
