using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class HIGHEST : MonoBehaviour
{
    public TMP_Text highestScore;

    private void Start()
    {
        highestScore.text = "HIGHEST: " + PlayerPrefs.GetInt("Score");
        PlayerController.Instance.OnPlayerDead += PlayerController_OnPlayerDead;
    }

    private void PlayerController_OnPlayerDead(object sender, System.EventArgs e)
    {
        int score = Score.Instance.GetScore();
        if (PlayerPrefs.GetInt("Score") < score)
        {
            PlayerPrefs.SetInt("Score", score);
            PlayerPrefs.Save();
            highestScore.text = "HIGHEST: " + PlayerPrefs.GetInt("Score");
        }
    }
}
