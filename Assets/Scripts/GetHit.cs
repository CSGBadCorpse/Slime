﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerController;

public class GetHit : MonoBehaviour
{
    public event EventHandler<EnemyPosEventArgs> OnPlayerHit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            OnPlayerHit?.Invoke(this, new EnemyPosEventArgs {//要知道碰到主角的史莱姆的位置
                x = collision.transform.position.x,
                y = collision.transform.position.y
            }) ;
        }
    }
}
