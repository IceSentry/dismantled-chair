﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepGoal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Yeah Goal");
        GameManager.Instance.EndGame(GameType.Sleep, 1);
    }
}
