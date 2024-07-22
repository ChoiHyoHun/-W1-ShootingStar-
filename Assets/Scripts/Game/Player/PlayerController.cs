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
    public ChargeBar chargeBar;

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
    //WallMove wallmove;
    Canvas canvas;

    //추가
    bool onMoving;
    [SerializeField] AnimationCurve dashWidthCurve;
    [SerializeField] AnimationCurve normalWidthCurve;
    [SerializeField] LayerMask whatIsPlatform;
    [SerializeField] float blinkDuration;

    Vector3 overlapOffset = new Vector3(0f, -5f, 0f);

    AudioSource playerAudio;

    //0: Beep 
    //1: Explosion
    //2: DashStart
    public AudioClip[] clips = new AudioClip[3];

    float beepDelay = 0.7f;
    float beepDelayRate = 0.7f;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        trail = GetComponentInChildren<TrailRenderer>();
        playerAudio = GetComponent<AudioSource>();

        //추가
        trail.widthCurve = normalWidthCurve;
        trail.time = 0.2f;

        //wallmove = GetComponent<WallMove>();

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
        float[] fixedXPositions = { -6.0f, -4.0f, -2.0f, 0.0f, 2.0f, 4.0f, 6.0f };

        // 현재 x 위치
        float currentX = transform.position.x;

        // 고정된 좌표 중 가장 가까운 값을 찾기위해 초기값 설정
        float closestX = fixedXPositions[0];
        float closestDistance = Mathf.Abs(currentX - closestX);

        //고정 좌표 배열을 순회하며 가장 가까운 좌표 찾기
        foreach (float x in fixedXPositions)
        {
            float distance = Mathf.Abs(currentX - x);
            if (distance < closestDistance)
            {
                //가장 가까운 거리라면 변수 업데이트
                closestDistance = distance;
                closestX = x;
            }
        }

        // 위치 보정
        Vector2 playerPos = transform.position;
        playerPos.x = closestX;
        transform.position = playerPos;


        rigid.velocity = new Vector2(rigid.velocity.x, rigid.velocity.y);
        //대쉬 상태가 아니면 정해진 최대 속도로 고정
        if (rigid.velocity.y < -maxFallingSpeed && !isDash)
        {
            rigid.velocity = new Vector2(rigid.velocity.x, -maxFallingSpeed);
        }
    }

    void LateUpdate()
    {

        velocityText.transform.position = new Vector3(transform.position.x, velocityText.transform.position.y, velocityText.transform.position.z);
    }

    void changeColor() //이후 Update()에서 호출
    {
        //ACCStep은 플레이어의 속도단계를 표현한 변수로 0, 1, 2, 3의 값을 가짐
        ACCStep = (int)(Mathf.Abs(rigid.velocity.y) / colorRange);
        if (ACCStep == colorStep)
        {
            ACCStep = colorStep - 1;
        }

        velocityText.SetText((ACCStep + 1).ToString());

        //아래 코루틴에서 targetColor를 참조하기 위해 기본값으로 초기화
        Color targetColor = sprite1.color;
        switch (ACCStep)
        {
            /*****
            기존 컬러코드
            0: #e0ffff
            1: #48d1cc
            2: #4169e1
            3: #000080

            *****/
            case 0:
                ColorUtility.TryParseHtmlString("#000080", out targetColor);

                //StartCoroutine(LerpColorChnage(sprite1.color, targetColor));
                //StartCoroutine(LerpTrailChnage(trail.startColor, targetColor));
                break;

            case 1:
                ColorUtility.TryParseHtmlString("#4169e1", out targetColor);

                //StartCoroutine(LerpColorChnage(sprite1.color, targetColor));
                //StartCoroutine(LerpTrailChnage(trail.startColor, targetColor));
                break;


            case 2:
                ColorUtility.TryParseHtmlString("#48d1cc", out targetColor);

                //StartCoroutine(LerpColorChnage(sprite1.color, targetColor));
                //StartCoroutine(LerpTrailChnage(trail.startColor, targetColor));
                break;


            case 3:
                ColorUtility.TryParseHtmlString("#e0ffff", out targetColor);

                //StartCoroutine(LerpColorChnage(sprite1.color, targetColor));
                //StartCoroutine(LerpTrailChnage(trail.startColor, targetColor));
                break;
        }
        StartCoroutine(LerpColorChnage(sprite1.color, targetColor));
        StartCoroutine(LerpTrailChnage(trail.startColor, targetColor));
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
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.DownArrow)) && chargeBar.currentGauge == chargeBar.maxGauge)
        {

            chargeBar.UseSkill();

            if (!isDash)
            {
                isDash = true;

                // 잠깐 정지
                rigid.velocity = Vector2.zero;

                //추가
                trail.widthCurve = dashWidthCurve;
                trail.time = 0.8f;

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
        float duration = 3f;
        float currentTime = 0;

        Coroutine blinkCoroutine = null;

        StartCoroutine(playBeepSound());
        playDashClip();

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

            if (currentTime > 2.2f)
            {
                StartCoroutine(GameManager.Instance.WhiteScreenEffect());
            }


            if (currentTime >= duration - 1f && blinkCoroutine == null)
            {
                blinkCoroutine = StartCoroutine(BlinkEffect());

            }

            yield return null;
        }

        if (isDash)
        {
            //대쉬 종료후 폭발 메서드 발동
            ExplosionAfterDash();

            isDash = false;

            StopCoroutine(playBeepSound());

            //추가
            trail.widthCurve = normalWidthCurve;
            trail.time = 0.2f;

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

    //플레이어 깜박임 효과
    IEnumerator BlinkEffect()
    {
        float blinkInterval;
        float elapsed = 0f;
        float startBlinkInterval = 0.08f;
        float endBlinkInterval = 0.01f; // 깜박임이 빨라질 최종 간격

        while (elapsed < blinkDuration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / blinkDuration;
            blinkInterval = Mathf.Lerp(startBlinkInterval, endBlinkInterval, t);

            SetSpriteVisibility((elapsed % (blinkInterval * 2)) < blinkInterval);
            yield return null;

        }

        SetSpriteVisibility(true);

    }

    void SetSpriteVisibility(bool isVisible)
    {
        float alpha = isVisible ? 1f : 0f;
        Color color1 = sprite1.color;
        color1.a = alpha;
        sprite1.color = color1;

        Color color2 = sprite2.color;
        color2.a = alpha;
        sprite2.color = color2;

        Color trailColor = trail.startColor;
        trailColor.a = alpha;
        trail.startColor = trailColor;
    }

    IEnumerator playBeepSound()
    {
        if (isDash)
        {
            float delay = beepDelay;
            while (delay > 0.01f)
            {
                playBeepClip();
                yield return new WaitForSeconds(delay);
                delay *= beepDelayRate;

            }
        }

    }

    void ExplosionAfterDash()
    {
        //폭발 범위 설정 및 폭발 범위 내부 플랫폼 배열로 반환
        Vector2 explosionRange = new Vector2(25f, 50f);
        Collider2D[] platforms = Physics2D.OverlapBoxAll(transform.position + overlapOffset, explosionRange, 0f, whatIsPlatform);
        playExplosionClip();
        for (int i = 0; i < platforms.Length; i++)
        {
            //플랫폼 코드를 분리하는 바람에 2개 다 받아와 메서드 호출 
            PlatformStaticHp staticPlatformScript = platforms[i].GetComponent<PlatformStaticHp>();
            PlatformRandHp randomPlatformScript = platforms[i].GetComponent<PlatformRandHp>();

            if (staticPlatformScript != null && randomPlatformScript == null)
            {
                staticPlatformScript.Break();
            }
            else if (randomPlatformScript != null && staticPlatformScript == null)
            {
                randomPlatformScript.Break();
            }
            else
            {
                Debug.Log("Not Platform");
            }



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

        chargeBar.DecreaseSkill(20f);

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

    //추가
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

    public void playBeepClip()
    {
        playerAudio.volume = 0.2f;
        playerAudio.PlayOneShot(clips[0]);
    }

    public void playExplosionClip()
    {
        playerAudio.volume = 1f;
        playerAudio.PlayOneShot(clips[1]);
    }

    public void playDashClip()
    {
        playerAudio.volume = 0.6f;
        playerAudio.PlayOneShot(clips[2]);
    }

}
