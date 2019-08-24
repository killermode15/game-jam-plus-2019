using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private int score;

    public void AddScore()
    {
        score++;
    }

    public void UpdateUI(TextMeshProUGUI scoreUI)
    {
        scoreUI.text = score.ToString();
    }
}
