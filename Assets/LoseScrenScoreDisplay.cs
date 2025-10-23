using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoseScrenScoreDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //your score can't change here so you don't need to update it more than the once
        TextMeshProUGUI uiText = GetComponent<TextMeshProUGUI>();
        uiText.text = "Your Score: " + ScoreCounter.score + "\nHigh Score: " + PlayerPrefs.GetInt("HighScore");
    }
}
