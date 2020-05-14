using UnityEngine;

public class UIView : MonoBehaviour
{
    [SerializeField]
    private GameObject ingameUI;
    [SerializeField]
    private GameSummaryView summaryView;
    [SerializeField]
    private ScoreView scoreView;

    public void SetCurrentScore(int score)
    {
        scoreView.SetCurrentScore(score);
    }

    public void ActivateIngameUI()
    {
        ToggleUIViews(GameStates.Game);
    }

    public void ShowGameSummary(bool gameWon, int score)
    {
        summaryView.SetSummaryText(gameWon, score);
        ToggleUIViews(GameStates.Summary);
    }

    private void ToggleUIViews(GameStates gamestate)
    {
        ingameUI.SetActive(gamestate == GameStates.Game);
        summaryView.gameObject.SetActive(gamestate == GameStates.Summary);
    }
}