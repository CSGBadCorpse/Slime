using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public static Score Instance;
    private int score;

    [SerializeField] private TMP_Text scoreText;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        score = 0;
        scoreText.text = "SCORE:" + score;
        GameManager.Instance.OnGameRestart += GameManager_OnGameRestart;
    }

    private void GameManager_OnGameRestart(object sender, System.EventArgs e)
    {
        score = 0;
    }

    private void Update()
    {
        scoreText.text = "SCORE:" + score;
    }




    public void AddScore(int s)
    {
        score += s;
    }

    public int GetScore()
    {
        return score;
    }
}
