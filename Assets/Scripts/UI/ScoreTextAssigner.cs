using System;
using TMPro;
using UnityEngine;

public class ScoreTextAssigner : MonoBehaviour
{
    public TMP_Text scoreText;

    private void Start()
    {
        GameManager.instance.scoreText = scoreText;
    }

    private void Update()
    {
        if (GameManager.instance.scoreText)
        {
            return;
        }
        else
        {
            GameManager.instance.scoreText = scoreText;
        }
    }
}
