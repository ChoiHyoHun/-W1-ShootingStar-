using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PopUpScore : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI popUpText;

    private void Awake()
    {
        popUpText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        StartCoroutine(textShowing(popUpText));
    }

    //점수가 양수면 + 붙이기
    public void SettingText(int score)
    {
        if (score > 0)
        {
            popUpText.SetText("+" + score.ToString());
        }
        else
        {
            //점수가 까질때는 텍스트 빨갛게 
            popUpText.color = Color.red;
            popUpText.SetText(score.ToString());
        }
    }

    IEnumerator textShowing(TextMeshProUGUI text)
    {
        yield return new WaitForSeconds(0.5f);
        float duration = 0f;
        while (duration < 0.5f)
        {
            duration += Time.deltaTime;
            text.color = Color.Lerp(text.color, new Color(text.color.r, text.color.g, text.color.b, 0), Time.unscaledDeltaTime);
            text.gameObject.transform.position += Vector3.up * Time.unscaledDeltaTime;

            yield return null;
        }
        //text.color = new Color(text.color.r, text.color.g, text.color.b, 1);
        text.gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
