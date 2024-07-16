using System.Collections;
using System.Collections.Generic;
using System.Timers;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : Singleton<PlayerController>
{
    Rigidbody2D rigid;
    TrailRenderer trail;
    [SerializeField] SpriteRenderer sprite1;
    [SerializeField] SpriteRenderer sprite2;
    ChargeBar chargeBar;

    [SerializeField] TextMeshProUGUI velocityTextPfb;
    TextMeshProUGUI velocityText;

    [SerializeField] float HorizontalSpeed;
    [SerializeField] float dashSpeed;
    [SerializeField] float bounceForce;
    [SerializeField] float maxFallingSpeed;
    [SerializeField] int colorStep;
    Vector3 rotationSpeed; // 초당 90도 회전 (Z축 기준)
    [SerializeField] float colorRange;
    float holdTime;
    public bool isDash;
    public int ACCStep;
    Coroutine dashCoroutine;
    Coroutine colorCoroutine;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        trail = GetComponentInChildren<TrailRenderer>();

        colorRange = maxFallingSpeed / colorStep;
        // slider = FindObjectOfType<Slider>();
        chargeBar = FindObjectOfType<ChargeBar>();

        velocityText = Instantiate(velocityTextPfb, GameObject.Find("Canvas").GetComponent<Canvas>().transform);
    }

    // Update is called once per frame
    void Update()
    {

        if (!isDash)
        {
            if (!isBouncing)
                changeColor();

            // 회전
            rotationSpeed = new Vector3(0, 0, (ACCStep + 1) * 180);
            transform.Rotate(rotationSpeed * Time.deltaTime);
        }
        else
        {
            if (transform.rotation != Quaternion.Euler(0, 0, 180))
                // 무조건 뾰족한거 아래로 향하게
                transform.rotation = Quaternion.Euler(0, 0, 180);
        }

    }

    void FixedUpdate()
    {
        if (!isDash)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            rigid.velocity = new Vector2(horizontalInput * HorizontalSpeed, rigid.velocity.y);
        }

        dash();

        if (rigid.velocity.y < -maxFallingSpeed)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, -maxFallingSpeed);
        }
    }

    void LateUpdate()
    {
        velocityText.transform.position = transform.position;
    }

    void changeColor()
    {
        ACCStep = (int)(Mathf.Abs(rigid.velocity.y) / colorRange);
        if (ACCStep == colorStep)
        {
            ACCStep = colorStep - 1;
        }

        velocityText.SetText((ACCStep + 1).ToString());

        Color targetColor;
        switch (ACCStep)
        {
            // 보라
            case 0:
                ColorUtility.TryParseHtmlString("#0000ff", out targetColor);

                // if (colorCoroutine != null)
                // {
                //     StopCoroutine(colorCoroutine);
                //     colorCoroutine = StartCoroutine(LerpColorChnage(sprite.color, targetColor));
                // }
                // else
                // {

                // }
                StartCoroutine(LerpColorChnage(sprite1.color, targetColor));
                // StartCoroutine(LerpColorChnage(sprite2.color, targetColor));
                StartCoroutine(LerpTrailChnage(trail.startColor, targetColor));
                break;

            case 1:
                ColorUtility.TryParseHtmlString("#008000", out targetColor);

                StartCoroutine(LerpColorChnage(sprite1.color, targetColor));
                // StartCoroutine(LerpColorChnage(sprite2.color, targetColor));
                StartCoroutine(LerpTrailChnage(trail.startColor, targetColor));
                break;


            case 2:
                ColorUtility.TryParseHtmlString("#ffff00", out targetColor);

                StartCoroutine(LerpColorChnage(sprite1.color, targetColor));
                // StartCoroutine(LerpColorChnage(sprite2.color, targetColor));
                StartCoroutine(LerpTrailChnage(trail.startColor, targetColor));
                break;


            case 3:
                ColorUtility.TryParseHtmlString("#ff0000", out targetColor);

                StartCoroutine(LerpColorChnage(sprite1.color, targetColor));
                // StartCoroutine(LerpColorChnage(sprite2.color, targetColor));
                StartCoroutine(LerpTrailChnage(trail.startColor, targetColor));
                break;
        }
    }

    IEnumerator LerpColorChnage(Color nowColor, Color targetColor)
    {
        float duration = 0.1f;
        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            sprite1.color = Color.Lerp(nowColor, targetColor, timeElapsed / duration);
            sprite2.color = Color.Lerp(nowColor, targetColor, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        sprite1.color = targetColor;
        sprite2.color = targetColor;
    }

    IEnumerator LerpTrailChnage(Color nowColor, Color targetColor)
    {
        float duration = 0.1f;
        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            trail.startColor = Color.Lerp(nowColor, targetColor, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        trail.startColor = targetColor;
    }

    void dash()
    {
        if (Input.GetKey(KeyCode.Space) && chargeBar.currentGauge > 0)
        {
            chargeBar.UseSkill();

            if (!isDash)
            {
                isDash = true;

                // 잠깐 정지
                rigid.velocity = Vector2.zero;

                if (dashCoroutine != null)
                {
                    StopCoroutine(dashCoroutine);
                    dashCoroutine = StartCoroutine(dashEffect());
                }
                else
                {
                    dashCoroutine = StartCoroutine(dashEffect());
                }


                velocityText.SetText("<#f98cde>Fever!");
            }

            holdTime += Time.deltaTime;

            float force = Mathf.Clamp(holdTime * dashSpeed, 0f, maxFallingSpeed);
            rigid.AddForce(Vector2.down * force, ForceMode2D.Impulse);

        }
        else
        {
            if (isDash)
            {
                isDash = false;
                StopCoroutine(dashCoroutine);
            }

            holdTime = 0f;
        }
    }

    IEnumerator dashEffect()
    {
        float timer = 0f;
        float duration = 0.5f;

        while (true)
        {
            timer += Time.deltaTime / duration;
            float hue = Mathf.Repeat(timer, 1f);  // hue 값이 0에서 1 사이를 반복
            Color newColor = Color.HSVToRGB(hue, 0.3f, 1f);  // HSV 값을 RGB로 변환
            sprite1.color = newColor;
            sprite2.color = newColor;
            trail.startColor = newColor;

            yield return null;  // 다음 프레임까지 대기
        }
    }

    Coroutine bounceCoroutine = null;
    float saveAcc;
    bool isBouncing = false;

    public void SaveAcc()
    {
        saveAcc = ACCStep;
        // Debug.Log("[Before]: " + saveAcc);
    }

    public void Bounce()
    {
        SaveAcc();

        rigid.velocity = Vector2.zero;
        rigid.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);

        if (bounceCoroutine != null)
        {
            StopCoroutine(bounceCoroutine);
            bounceCoroutine = StartCoroutine(checkBounceReverse());
        }
        else
        {
            bounceCoroutine = StartCoroutine(checkBounceReverse());
        }
    }

    IEnumerator checkBounceReverse()
    {
        while (true)
        {
            if (!isBouncing)
            {
                isBouncing = true;
                velocityText.SetText("");
            }


            if (rigid.velocity.y < 0)
            {
                if (isBouncing)
                {
                    isBouncing = false;
                    // velocityText.SetText("");
                }
                float target_vel = ((saveAcc - 1) < 0 ? 0 : (saveAcc - 1)) * colorRange;

                // Debug.Log("[After]: " + target_vel);
                // Debug.Log("=================================");
                rigid.velocity = new Vector2(rigid.velocity.x, -target_vel);
                yield break;
            }

            yield return null;
        }
    }


}
