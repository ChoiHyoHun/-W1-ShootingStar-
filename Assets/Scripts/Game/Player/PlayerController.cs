using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    Rigidbody2D rigid;
    TrailRenderer trail;
    SpriteRenderer sprite;

    [SerializeField] float HorizontalSpeed;
    [SerializeField] float dashSpeed;
    [SerializeField] float bounceForce;
    [SerializeField] float maxFallingSpeed;
    [SerializeField] int colorStep;
    Vector3 rotationSpeed = new Vector3(0, 0, 90); // 초당 90도 회전 (Z축 기준)
    float colorRange;
    float holdTime;
    public bool isDash;
    public int ACCStep;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        trail = GetComponentInChildren<TrailRenderer>();

        colorRange = (int)(maxFallingSpeed / colorStep);
    }

    // Update is called once per frame
    void Update()
    {
        changeColor();

        // 회전
        if (!isDash)
        {
            // Time.deltaTime을 곱하여 프레임 속도에 관계없이 일정 속도로 회전
            transform.Rotate(rotationSpeed * Time.deltaTime);

            if (rigid.constraints != RigidbodyConstraints2D.None)
                rigid.constraints = RigidbodyConstraints2D.None;
        }
        else
        {
            if (transform.rotation != Quaternion.Euler(0, 0, 180))
                // 무조건 뾰족한거 아래로 향하게
                transform.rotation = Quaternion.Euler(0, 0, 180);

            if (rigid.constraints != RigidbodyConstraints2D.FreezeRotation)
                rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

    }

    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        rigid.AddForce(Vector2.right * horizontalInput * HorizontalSpeed, ForceMode2D.Impulse);

        dash();

        if (rigid.velocity.y < -maxFallingSpeed)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, -maxFallingSpeed);
        }
    }

    void changeColor()
    {
        int ACCStep = (int)(Mathf.Abs(rigid.velocity.y) / colorRange);
        if (ACCStep == colorStep)
        {
            ACCStep = colorStep - 1;
        }

        Color targetColor;

        switch (ACCStep)
        {
            // 보라
            case 0:
                ColorUtility.TryParseHtmlString("#0000ff", out targetColor);

                StartCoroutine(LerpColorChnage(sprite.color, targetColor));
                StartCoroutine(LerpTrailChnage(trail.startColor, targetColor));
                break;

            case 1:
                ColorUtility.TryParseHtmlString("#008000", out targetColor);

                StartCoroutine(LerpColorChnage(sprite.color, targetColor));
                StartCoroutine(LerpTrailChnage(trail.startColor, targetColor));
                break;


            case 2:
                ColorUtility.TryParseHtmlString("#ffff00", out targetColor);

                StartCoroutine(LerpColorChnage(sprite.color, targetColor));
                StartCoroutine(LerpTrailChnage(trail.startColor, targetColor));
                break;


            case 3:
                ColorUtility.TryParseHtmlString("ff0000", out targetColor);

                StartCoroutine(LerpColorChnage(sprite.color, targetColor));
                StartCoroutine(LerpTrailChnage(trail.startColor, targetColor));
                break;


                // case 4:
                //     ColorUtility.TryParseHtmlString("#ffff00", out targetColor);

                //     StartCoroutine(LerpColorChnage(sprite.color, targetColor));
                //     StartCoroutine(LerpTrailChnage(trail.startColor, targetColor));
                //     break;

                // case 5:
                //     ColorUtility.TryParseHtmlString("#ff8c00", out targetColor);

                //     StartCoroutine(LerpColorChnage(sprite.color, targetColor));
                //     StartCoroutine(LerpTrailChnage(trail.startColor, targetColor));
                //     break;

                // case 6:
                //     ColorUtility.TryParseHtmlString("#ff0000", out targetColor);

                //     StartCoroutine(LerpColorChnage(sprite.color, targetColor));
                //     StartCoroutine(LerpTrailChnage(trail.startColor, targetColor));
                //     break;
        }
    }

    IEnumerator LerpColorChnage(Color nowColor, Color targetColor)
    {
        float duration = 0.1f;
        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            sprite.color = Color.Lerp(nowColor, targetColor, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        sprite.color = targetColor;
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
        if (Input.GetKey(KeyCode.Space))
        {
            if (!isDash)
            {
                isDash = true;

                // 잠깐 정지
                rigid.velocity = Vector2.zero;

                // 무조건 뾰족한거 아래로 향하게
                // transform.rotation = Quaternion.Euler(0, 0, 180);
                // rigid.constraints = RigidbodyConstraints2D.FreezeRotation;
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
                // rigid.constraints = RigidbodyConstraints2D.None;
            }

            holdTime = 0f;
        }
    }


}
