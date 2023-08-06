using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartUI : MonoBehaviour
{

    private void Start()
    {
        PlayerController.Instance.OnPlayerDead += PlayerController_OnPlayerDead;
        GameManager.Instance.OnGameRestart += GameManager_OnGameRestart;
        Hide();
    }

    private void GameManager_OnGameRestart(object sender, System.EventArgs e)
    {
        Hide();
    }

    private void PlayerController_OnPlayerDead(object sender, System.EventArgs e)
    {
        Show();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
}

