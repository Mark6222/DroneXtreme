using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    [SerializeField] private int score;

    void Start()
    {
        score = 0;
    }

    public void AddPoints(int points)
    {
        score += points;
    }

    public int GetScore()
    {
        return score;
    }
}
