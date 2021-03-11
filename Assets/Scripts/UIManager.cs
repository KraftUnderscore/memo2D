using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText_;
    [SerializeField]
    private GameObject gameOverPanel_;
    [SerializeField]
    private GameObject victoryPanel_;

    public void UpdateScore(int score)
    {
        scoreText_.text = score.ToString();
    }

    public void DisplayGameOver()
    {
        gameOverPanel_.SetActive(true);
    }

    public void DisplayVictory()
    {
        victoryPanel_.SetActive(true);
    }

    public void ResetUI()
    {
        scoreText_.text = 0.ToString();
        gameOverPanel_.SetActive(false);
        victoryPanel_.SetActive(false);
    }
}
