using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalZone : MonoBehaviour
{
    public GameObject LevelCompleteText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            LevelComplete();
        }
    }

    private void LevelComplete()
    {
        LevelCompleteText.SetActive(true);
        Time.timeScale = 0;
    }
}
