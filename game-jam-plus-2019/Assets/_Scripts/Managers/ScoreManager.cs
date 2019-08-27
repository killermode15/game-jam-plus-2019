using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private int score;

    [SerializeField] private string scoreUI;

    public int GetScore => score;

    public void SetScore(int value)
    {
        score = value;
    }

    private void Update()
    {
        scoreUI = score.ToString();
    }

    public void AddScore()
    {
        score++;
    }

    public void UpdateUI(TextMeshProUGUI scoreText)
    {
        scoreText.text = scoreUI;
    }

    public void Restart()
    {
        scoreUI = "";
    }
}
