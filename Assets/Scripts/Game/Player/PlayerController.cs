using System.Collections;
using JetBrains.Annotations;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    Rigidbody2D rigid;
    TrailRenderer trail;
    [SerializeField] SpriteRenderer sprite1;
    [SerializeField] SpriteRenderer sprite2;
    ChargeBar chargeBar;

    [SerializeField] TextMeshProUGUI velocityTextPfb;
    TextMeshProUGUI velocityText;

    [SerializeField] ParticleSystem particle;

    [SerializeField] float HorizontalSpeed;
    [SerializeField] float dashSpeed;
    [SerializeField] float bounceForce;
    [SerializeField] float maxFallingSpeed;

    [SerializeField] int colorStep;
    Vector3 rotationSpeed; // 초당 90도 회전 (Z축 기준)
    [SerializeField] float colorRange;
    public bool isDash;
    public int ACCStep;
    Coroutine dashCoroutine;
    Coroutine bounceCoroutine = null;
    float saveAcc;
    bool isBouncing = false;
    WallMove wallmove;
    Canvas canvas;

    //추가
    bool onMoving;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        trail = GetComponentInChildren<TrailRenderer>();
        wallmove = GetComponent<WallMove>();

        colorRange = maxFallingSpeed / colorStep;
        // slider = FindObjectOfType<Slider>();
        chargeBar = FindObjectOfType<ChargeBar>();

        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        if (velocityTextPfb != null)
        {
            velocityText = Instantiate(velocityTextPfb, new Vector3(0, 4, 0), Quaternion.identity, canvas.transform);
            velocityText.transform.SetAsFirstSibling();
        }

        onMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        dash();

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

        //추가
        //float horizontalInput = Input.GetAxis("Horizontal");
        if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && !onMoving)
        {
            StartCoroutine(MoveRight());
        }
        else if ((Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) && !onMoving)
        {
            StartCoroutine(MoveLeft());
        }


        /*
                if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
                {
                    Debug.Log("Left");
                    Vector2 playerPos = transform.position;
                    playerPos.x -= 2.0f;
                    transform.position = playerPos;

                }

                if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
                {
                    Debug.Log("Right");
                    Vector2 playerPos = transform.position;
                    playerPos.x += 2.0f;
                    transform.position = playerPos;
                }
        */
    }

    void FixedUpdate()
    {
        //추가
        //위치 보정 코드들 
        if (transform.position.x < -5 && transform.position.x > -7)
        {
            Vector2 playerPos = transform.position;
            playerPos.x = -6.0f;
            transform.position = playerPos;
        }

        if (transform.position.x < -3 && transform.position.x > -5)
        {
            Vector2 playerPos = transform.position;
            playerPos.x = -4.0f;
            transform.position = playerPos;
        }

        if (transform.position.x < -1 && transform.position.x > -3)
        {
            Vector2 playerPos = transform.position;
            playerPos.x = -2.0f;
            transform.position = playerPos;
        }

        if (transform.position.x < 1 && transform.position.x > -1)
        {
            Vector2 playerPos = transform.position;
            playerPos.x = 0f;
            transform.position = playerPos;
        }

        if (transform.position.x < 3 && transform.position.x > 1)
        {
            Vector2 playerPos = transform.position;
            playerPos.x = 2.0f;
            transform.position = playerPos;
        }

        if (transform.position.x < 5 && transform.position.x > 3)
        {
            Vector2 playerPos = transform.position;
            playerPos.x = 4.0f;
            transform.position = playerPos;
        }

        if (transform.position.x < 7 && transform.position.x > 5)
        {
            Vector2 playerPos = transform.position;
            playerPos.x = 6.0f;
            transform.position = playerPos;
        }


        rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y);
        if (rigid.velocity.y < -maxFallingSpeed)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, -maxFallingSpeed);
        }
    }

    void LateUpdate()
    {
        Debug.Log(rigid.velocity.y);
        velocityText.transform.position = new Vector3(transform.position.x, velocityText.transform.position.y, velocityText.transform.position.z);
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
            case 0:
                ColorUtility.TryParseHtmlString("#e0ffff", out targetColor);

                StartCoroutine(LerpColorChnage(sprite1.color, targetColor));
                StartCoroutine(LerpTrailChnage(trail.startColor, targetColor));
                break;

            case 1:
                ColorUtility.TryParseHtmlString("#48d1cc", out targetColor);

                StartCoroutine(LerpColorChnage(sprite1.color, targetColor));
                StartCoroutine(LerpTrailChnage(trail.startColor, targetColor));
                break;


            case 2:
                ColorUtility.TryParseHtmlString("#4169e1", out targetColor);

                StartCoroutine(LerpColorChnage(sprite1.color, targetColor));
                StartCoroutine(LerpTrailChnage(trail.startColor, targetColor));
                break;


            case 3:
                ColorUtility.TryParseHtmlString("#000080", out targetColor);

                StartCoroutine(LerpColorChnage(sprite1.color, targetColor));
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
        //대쉬 조건이 스페이스바 없이 게이지가 다 차면 발동으로 변경
        //기존 조건 (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.DownArrow)) && chargeBar.currentGauge == chargeBar.maxGauge
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.DownArrow)) && chargeBar.currentGauge == chargeBar.maxGauge)
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

            StartCoroutine(dashing());

        }
    }

    IEnumerator dashing()
    {
        float duration = 2f;
        float currentTime = 0;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;

            //대쉬 속도 = 4단계 속도가 아니라 대쉬 전용 속도로 변환
            /*
            if (Mathf.Abs(rigid.velocity.y) < maxFallingSpeed)
                rigid.velocity = new Vector2(rigid.velocity.x, -maxFallingSpeed);
            */
            if (Mathf.Abs(rigid.velocity.y) < dashSpeed)
                rigid.velocity = new Vector2(rigid.velocity.x, -dashSpeed);

            yield return null;
        }

        if (isDash)
        {
            isDash = false;
            StopCoroutine(dashCoroutine);
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

    float blockY;
    public void SaveAcc()
    {
        saveAcc = ACCStep;

        // Debug.Log("[Before]: " + saveAcc);
    }

    public void Bounce(float yPos)
    {
        SaveAcc();

        chargeBar.DecreaseSkill(10f);

        if (rigid.position.y < yPos)
        {
            rigid.velocity = Vector2.zero;
            float target_vel = ((saveAcc - 1) < 0 ? 0 : (saveAcc - 1)) * colorRange;

            rigid.velocity = new Vector2(rigid.velocity.x, -target_vel);
        }
        else
        {
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
                }
                float target_vel = ((saveAcc - 1) < 0 ? 0 : (saveAcc - 1)) * colorRange;

                rigid.velocity = new Vector2(rigid.velocity.x, -target_vel);
                yield break;
            }

            yield return null;
        }
    }

    IEnumerator MoveRight()
    {

        Vector2 playerPos = transform.position;
        if (playerPos.x < 6)
        {
            onMoving = true;
            playerPos.x += 2.0f;
            transform.position = playerPos;
            yield return new WaitForSeconds(0.12f);
            onMoving = false;
        }

    }

    IEnumerator MoveLeft()
    {
        Vector2 playerPos = transform.position;
        if (playerPos.x > -6)
        {
            onMoving = true;
            playerPos.x -= 2.0f;
            transform.position = playerPos;
            yield return new WaitForSeconds(0.12f);
            onMoving = false;
        }

    }

    void OnDestroy()
    {
        ParticleSystem p = Instantiate(particle, transform.position, quaternion.identity);
        var module = p.main;
        module.startColor = sprite1.color;
        Destroy(velocityText.gameObject);
    }

}
