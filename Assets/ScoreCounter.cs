using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreCounter : MonoBehaviour
{
    //reusing a little bit of applepicker code for this one too
    
    [Header("Dynamic")]
    static public int score; //so the code can access the score in the losing screen scene
    private TextMeshProUGUI uiText;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        uiText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        uiText.text = "Score: " + score.ToString("#,0");
    }
}
