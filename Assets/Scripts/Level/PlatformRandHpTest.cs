using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using JetBrains.Annotations;
using TMPro;
using UnityEngine.UI;

public class PlatformRandHpTest : MonoBehaviour
{
    [SerializeField]
    private int hp;
    private int hpLimit = 4;
    private BoxCollider2D boxCollider;

    SpriteRenderer pRenderer;

    /*
    [SerializeField]
    TextMeshProUGUI hpTextPfb;
    TextMeshProUGUI hpText;
    Canvas fixedCanvas;
    */

    TextMeshProUGUI hpText;

    //색들
    string greenHex = "#008000";
    Color green;

    private Color ColorCodeToColor(string colorCode, Color color)
    {
        if (ColorUtility.TryParseHtmlString(colorCode, out color))
        {
            return color;
        }
        else
        {
            Debug.Log("틀린 컬러 코드");
            return color;
        }
    }

    private void Awake()
    {
        hpText = GetComponentInChildren<TextMeshProUGUI>();
        /*
        fixedCanvas = GameObject.Find("FixedCanvas").GetComponent<Canvas>();
        */
    }

    //바닥 생성되는 순간 호출 > 이후 Start()호출
    private void OnEnable()
    {
        RandomSpawn();
        //OnEnable에서 정해진 체력에 따라 색상 변경 및 물리 머티리얼 할당
        HpToColor();
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

    private void Start()
    {

    }

    private void SetHpText()
    {
        hpText.text = hp.ToString();
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (collisionInfo.collider.tag == "Player")
        {
            if (CanBreak())
            {
                if (!PlayerController.Instance.isDash)
                {
                    PlayerController.Instance.Bounce();
                }
                GameManager.Instance.AddScore(CalculateScore());
                gameObject.SetActive(false);
                //Destroy(hpText.gameObject);
            }
            else
            {
                // 임시로 죽으면 게임 멈춤
                Debug.Log("YOU DIED");
                // GameManager.Instance.FailGame();

            }

        }
    }

    private void RandomSpawn()
    {
        if (Random.Range(0, 2) == 0)
        {
            gameObject.SetActive(true);
            hp = Random.Range(0, hpLimit);
            SetHpText();
            /*
            hpText = Instantiate(hpTextPfb, transform.position, Quaternion.identity, fixedCanvas.GetComponent<Canvas>().transform);
            hpText.SetText(hp.ToString());
            */
        }
        else
        {
            //Destroy(hpText.gameObject);
            gameObject.SetActive(false);
        }
    }

    private void HpToColor()
    {
        pRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();


        //체력에 따라 색상을 변경
        switch (hp)
        {
            case 0: //파
                pRenderer.color = Color.blue;
                break;
            case 1: //초
                pRenderer.color = ColorCodeToColor(greenHex, green);
                break;
            case 2: //노
                pRenderer.color = Color.yellow;
                break;
            case 3: //빨
                pRenderer.color = Color.red;
                break;
        }
    }

    private int CalculateScore()
    {
        int basicPoint = 100;
        return basicPoint * (1 + hp);

    }

    private bool CanBreak()
    {
        if (PlayerController.Instance.ACCStep >= hp || PlayerController.Instance.isDash)
        {
            // PlayerController.Instance.SaveBounce();
            return true;
        }
        else
        {
            return false;
        }

    }

}
