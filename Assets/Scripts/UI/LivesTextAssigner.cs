using System;
using TMPro;
using UnityEngine;

public class LivesTextAssigner : MonoBehaviour
{
    public TMP_Text livesText;

    private void Start()
    {
        GameManager.instance.livesText = livesText;
    }

    private void Update()
    {
        if (GameManager.instance.livesText)
        {
            return;
        }
        else
        {
            GameManager.instance.livesText = livesText;
        }
    }
}
