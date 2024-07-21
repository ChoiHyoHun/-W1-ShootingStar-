using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using JetBrains.Annotations;
using TMPro;
using UnityEngine.UI;

public class PlatformRandHp : MonoBehaviour
{
    [SerializeField]
    private int hp;
    private int hpLimit = 4;
    private BoxCollider2D boxCollider;

    SpriteRenderer pRenderer;


    public PopUpScore scoreTextPfb;
    private PopUpScore scoreText;

    TextMeshProUGUI hpText;

    //색들
    string colorCode0 = "#e0ffff";
    Color color0;
    string colorCode1 = "#48d1cc";
    Color color1;
    string colorCode2 = "#4169e1";
    Color color2;
    string colorCode3 = "#000080";
    Color color3;

    //추가
    [SerializeField]
    private float rayDistance = 5f;
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

                PlayerController.Instance.chargeBar.ChargeSkill(2f);
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
            return 700;
        }
        else if (distance <= 2f && distance > 1f)
        {
            return 500;
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

    }

    //바닥 생성되는 순간 호출 > 이후 Start()호출
    private void OnEnable()
    {
        RandomSpawn();
        //OnEnable에서 정해진 체력에 따라 색상 변경 및 물리 머티리얼 할당
        HpToColor();
    }

    private void Start()
    {

    }

    private void SetHpText()
    {
        hpText.text = (hp + 1).ToString();
    }

    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (collisionInfo.collider.tag == "Player")
        {
            if (CanBreak())
            {
                if (!PlayerController.Instance.isDash)
                {
                    PlayerController.Instance.Bounce(transform.position.y);
                }
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

    private void RandomSpawn()
    {
        if (Random.Range(0.0f, 1.0f) >= 0.4)
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
                pRenderer.color = ColorCodeToColor(colorCode0, color0);
                break;
            case 1: //초
                pRenderer.color = ColorCodeToColor(colorCode1, color1);
                break;
            case 2: //노
                pRenderer.color = ColorCodeToColor(colorCode2, color2);
                break;
            case 3: //빨
                pRenderer.color = ColorCodeToColor(colorCode3, color3);
                break;
        }
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
            // PlayerController.Instance.SaveBounce();
            return true;
        }
        else
        {
            return false;
        }

    }

}
