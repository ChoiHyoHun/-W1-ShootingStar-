using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    
    public TMP_Text scoreText;
    private int totalScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }


    void Update()
    {

        
    }
    public void DisplayScore(int score) {
        totalScore += score;
    }
}