using UnityEngine;
using UnityEngine.UI;

public class LevelButtonView : MonoBehaviour
{
    public int LevelID;
    public Text HighscoreText;
    public GameObject StarImage;

    public void SetHighscore(string highscoreText, bool gameWon)
    {
        HighscoreText.text = highscoreText;
        StarImage.SetActive(gameWon);
    }
}