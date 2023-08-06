using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    private Image healthUI;
    private void Start()
    {
        healthUI = GetComponent<Image>();
        PlayerController.Instance.OnPlayerHealthChanged += Player_OnPlayerHealthChanged;
        GameManager.Instance.OnGameRestart += GameManager_OnGameRestart;
    }

    private void GameManager_OnGameRestart(object sender, System.EventArgs e)
    {
        healthUI.fillAmount = 1;
    }

    private void Player_OnPlayerHealthChanged(object sender, PlayerController.PlayerHealthEventArgs e)
    {
        healthUI.fillAmount = e.healthProgress;
    }

}
