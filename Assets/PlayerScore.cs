using UnityEngine;

public class PlayerScore : MonoBehaviour
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

    public int GetScore()
    {
        return score;
    }

    public void AddToScoreToAdd(int addScore)
    {
        scoreToAdd = addScore;
    }

    public int GetScoreToAdd()
    {
        return scoreToAdd;
    }
    public void ResetScoreToAdd()
    {
        scoreToAdd = 0;
    }
}
