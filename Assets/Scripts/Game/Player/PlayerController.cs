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
    [SerializeField] int hpStep;
    int colorBlock;

    Coroutine colorChanging;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        trail = GetComponentInChildren<TrailRenderer>();

        colorBlock = (int)(maxFallingSpeed / hpStep);
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

        if (Mathf.Abs(rigid.velocity.y) > maxFallingSpeed)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, maxFallingSpeed);
        }
    }

    void changeColor()
    {
        float hp = Mathf.Abs(rigid.velocity.y);

        int nowColor = (int)(hp / colorBlock);
        Color targetColor;

        switch (nowColor)
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

            default:
                Debug.Log("Wrong");
                break;
        }
    }

    IEnumerator LerpColorChnage(Color nowColor, Color targetColor)
    {
        float duration = 0.5f;
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
        float duration = 0.5f;
        float timeElapsed = 0;

        while (timeElapsed < duration)
        {
            trail.startColor = Color.Lerp(nowColor, targetColor, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        sprite.color = targetColor;
    }

    void dash()
    {
        // slide가 있다면
        if (Input.GetKeyDown(KeyCode.Space))
        {

        }
    }

}
