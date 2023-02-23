using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TMP_Text[] highScoreTexts;
    private int  playerHighScore;
    
    void Start()
    {
        playerHighScore = Board.instance.highScore;
    }

    void Update()
    {
        foreach (TMP_Text highText in highScoreTexts)
        {
            highText.text = PlayerPrefs.GetInt("HighScore").ToString();

        }
    }
}
