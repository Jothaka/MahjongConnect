using UnityEngine;
using UnityEngine.UI;

public class ScoreView : MonoBehaviour
{
    [SerializeField]
    private Text scoreText;

    public void SetCurrentScore(int score)
    {
        scoreText.text = score.ToString();
    }
}