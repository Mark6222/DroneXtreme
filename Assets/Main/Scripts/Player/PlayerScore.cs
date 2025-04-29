using System.Collections;
using UnityEngine;

public class PlayerScore
{
    [SerializeField] private int score;
    [SerializeField] private int scoreToAdd;

    void Start()
    {
        score = 0;
        scoreToAdd = 0;
    }

    public void AddPoints()
    {
        score += scoreToAdd;
    }

    public void AddPointsForTracking(int addScore)
    {
        score += addScore;
    }

    public int GetScore()
    {
        return score;
    }

    public void AddToScoreToAdd(int addScore)
    {
        scoreToAdd = scoreToAdd + addScore;
    }

    public int GetScoreToAdd()
    {
        return scoreToAdd;
    }
    public void ResetScoreToAdd()
    {
        scoreToAdd = 0;
    }

    public void ResetScore()
    {
        score = 0;
    }
}
