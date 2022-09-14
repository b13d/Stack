using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    enum nameText 
    {
        scoreText,
        recordText
    }

    int scoreCurrent;
    //public static int scoreCurrent;
    List<TextMeshProUGUI> texts = new List<TextMeshProUGUI>();
    



    private void Awake()
    {
        var allTexts = GetComponentsInChildren<TextMeshProUGUI>();

        for (int i = 0; i < allTexts.Length; i++)
        {
            texts.Add(allTexts[i]);
        }
        


        SettingValues();
    }


    //PlayerPrefs scorePoints, maxScore;

    void SettingValues()
    {
        texts[(int)nameText.scoreText].text = "Score = " + scoreCurrent; // current value
        texts[(int)nameText.recordText].text = "Record = " + PlayerPrefs.GetInt("maxScore"); // record value
    }

    internal void ChangeScore()
    {
        scoreCurrent++;

        PlayerPrefs.SetInt("scorePoints", scoreCurrent);

        if (PlayerPrefs.GetInt("maxScore") < scoreCurrent)
        {
            PlayerPrefs.SetInt("maxScore", scoreCurrent);
        }

        SettingValues();
    }
}
