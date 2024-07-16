using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    Rigidbody2D rigid;
    TrailRenderer trail;
    SpriteRenderer sprite;

    [SerializeField] float HorizontalSpeed;
    [SerializeField] float maxFallingSpeed;
    [SerializeField] float dashSpeed;
    [SerializeField] float bounceForce;
    [SerializeField] int colorStep;
    int colorBlock;
    float holdTime;
    public bool isDash;
    public int ACCStep;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        trail = GetComponentInChildren<TrailRenderer>();

        colorBlock = (int)(maxFallingSpeed / colorStep);
    }

    // Update is called once per frame
    void Update()
    {
        changeColor();
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
        float acc = Mathf.Abs(rigid.velocity.y);
        ACCStep = (int)(acc / colorBlock);

        Color targetColor;

        switch (ACCStep)
        {
            // 보라
            case 0:
                ColorUtility.TryParseHtmlString("#800080", out targetColor);

                StartCoroutine(LerpColorChnage(sprite.color, targetColor));
                StartCoroutine(LerpTrailChnage(trail.startColor, targetColor));
                break;

            case 1:
                ColorUtility.TryParseHtmlString("#4b0082", out targetColor);

                StartCoroutine(LerpColorChnage(sprite.color, targetColor));
                StartCoroutine(LerpTrailChnage(trail.startColor, targetColor));
                break;


            case 2:
                ColorUtility.TryParseHtmlString("#0000ff", out targetColor);

                StartCoroutine(LerpColorChnage(sprite.color, targetColor));
                StartCoroutine(LerpTrailChnage(trail.startColor, targetColor));
                break;


            case 3:
                ColorUtility.TryParseHtmlString("#008000", out targetColor);

                StartCoroutine(LerpColorChnage(sprite.color, targetColor));
                StartCoroutine(LerpTrailChnage(trail.startColor, targetColor));
                break;


            case 4:
                ColorUtility.TryParseHtmlString("#ffff00", out targetColor);

                StartCoroutine(LerpColorChnage(sprite.color, targetColor));
                StartCoroutine(LerpTrailChnage(trail.startColor, targetColor));
                break;

            case 5:
                ColorUtility.TryParseHtmlString("#ff8c00", out targetColor);

                StartCoroutine(LerpColorChnage(sprite.color, targetColor));
                StartCoroutine(LerpTrailChnage(trail.startColor, targetColor));
                break;

            case 6:
                ColorUtility.TryParseHtmlString("#ff0000", out targetColor);

                StartCoroutine(LerpColorChnage(sprite.color, targetColor));
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
            }

            holdTime += Time.deltaTime;

            float force = Mathf.Clamp(holdTime * dashSpeed, 0f, maxFallingSpeed);
            rigid.AddForce(Vector2.down * force, ForceMode2D.Impulse);
        }
        else
        {
            if (isDash)
                isDash = false;

            holdTime = 0f;
        }
    }

    public void Bounce(int power)
    {
        rigid.AddForce(Vector2.up * power * bounceForce, ForceMode2D.Impulse);
    }

}
