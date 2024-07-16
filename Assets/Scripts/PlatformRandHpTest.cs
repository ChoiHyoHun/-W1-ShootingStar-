using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using JetBrains.Annotations;
public class PlatformRandHpTest : MonoBehaviour
{
    [SerializeField]
    private int hp = 0;
    private int hpLimit = 4;

    public PhysicsMaterial2D[] physicsMaterials;
    private BoxCollider2D boxCollider;

    SpriteRenderer pRenderer;

    //색들
    string greenHex = "#008000";
    Color green;

    private Color ColorCodeToColor(string colorCode, Color color){
        if(ColorUtility.TryParseHtmlString(colorCode, out color))
        {
            return color;
        } else
        {
            Debug.Log("틀린 컬러 코드");
            return color;
        }
    }

    //바닥 생성되는 순간 호출 > 이후 Start()호출
    private void OnEnable()
    {
        RandomSpawn();
        hp = Random.Range(0, hpLimit );
    }


    private void Start()
    {
        //OnEnable에서 정해진 체력에 따라 색상 변경 및 물리 머티리얼 할당
        HpToColor();
    }

    private void RandomSpawn()
    {
        if(Random.Range(0, 2)==0){
            gameObject.SetActive(true);
        } else {
            gameObject.SetActive(false);
        }
    }

        private void HpToColor()
        {
        pRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();


        //체력에 따라 색상을 변경
        switch(hp){
            case 0: //파
            pRenderer.color = Color.blue;
            boxCollider.sharedMaterial = physicsMaterials[0];
            break;
            case 1: //초
            pRenderer.color = ColorCodeToColor(greenHex, green);
            boxCollider.sharedMaterial = physicsMaterials[1];
            break;
            case 2: //노
            pRenderer.color = Color.yellow;
            boxCollider.sharedMaterial = physicsMaterials[2];
            break;
            case 3: //빨
            pRenderer.color = Color.red;
            boxCollider.sharedMaterial = physicsMaterials[3];
            break;
        }
    }

}
