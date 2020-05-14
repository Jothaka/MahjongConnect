using UnityEngine;
using UnityEngine.UI;

public class GameSummaryView : MonoBehaviour
{
    public Text Summary;

    public void SetSummaryText(bool gameWon, int score)
    {
        string summaryText;

        if (gameWon)
            summaryText = string.Format("Congratulations you got {0} Points this game!", score);
        else
            summaryText = string.Format("You lost with {0} Points. Better luck next time!", score);

        Summary.text = summaryText;
    }
}
