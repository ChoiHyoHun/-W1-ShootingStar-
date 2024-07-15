using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformRandHpTest : MonoBehaviour
{
    public int hp = 0;
    private int hpLimit = 7;

    SpriteRenderer pRenderer;

    private void OnEnable()
    {
        hp = Random.Range(0, hpLimit );
    }

    private void Start()
    {
        pRenderer = GetComponent<SpriteRenderer>();

        switch(hp){
            case 0: //보
            pRenderer.color = Color.black;
            break;

            case 1: //남
            pRenderer.color = new Color(0, 0, 153);
            break;

            case 2: //파
            pRenderer.color = Color.blue;
            break;

            case 3: //초
            pRenderer.color = Color.green;
            break;

            case 4: //노
            pRenderer.color = Color.yellow;
            break;

            case 5: //주
            pRenderer.color = new Color(255, 128, 0);
            break;

            case 6: //빨
            pRenderer.color = Color.red;
            break;

        }
    }
}
