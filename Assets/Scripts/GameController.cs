using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameCreatorController CreatorController;
    public UIView UiView;

    public int ScorePerMatch = 15;
    public int ScoreLossPerFail = 10;

    private List<Tile> allTiles = new List<Tile>();
    private List<Tile> interactableTiles = new List<Tile>();

    private Tile currentSelectedTile;

    private Level currentLevel;
    private Tile[] currentHint;

    private void Start()
    {
        currentLevel = new Level() { LevelID = GameManager.Instance.LevelID };

        CreatorController.OnCreationFinished += OnCreationFinished;
        CreatorController.CreateLevel(currentLevel.LevelID);
    }

    //triggered by UI-Event
    public void OnLeaveButtonClick()
    {
        UiView.ShowGameSummary(false, currentLevel.CurrentHighscore);
    }

    //triggered by UI-Event
    public void OnGameSummaryButtonClick()
    {
        ReturnToMenu();
    }

    //triggered by UI-Event
    public void OnHintButtonClick()
    {
        currentHint[0].Highlight();
        currentHint[1].Highlight();
    }

    private void OnCreationFinished(List<Tile> generatedTiles)
    {
        interactableTiles.Clear();
        allTiles.Clear();
        for (int i = 0; i < generatedTiles.Count; i++)
        {
            if (generatedTiles[i].TileFace != TileFaces.Empty)
            {
                generatedTiles[i].View.OnTileClicked += OnTileClicked;
                interactableTiles.Add(generatedTiles[i]);
            }
        }
        allTiles.AddRange(generatedTiles);
        currentHint = PathCalculator.GetValidPathPair(interactableTiles);
    }

    private void ReturnToMenu()
    {
        if (IsNewHighscore())
            HighscoreUtility.SaveHighscore(currentLevel);

        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    
    private bool IsNewHighscore()
    {
        bool newHighscore = false;

        Level previousHighscore = HighscoreUtility.LoadSpecificHighscore(currentLevel.LevelID);

        if (previousHighscore == null)
            newHighscore = true;
        else if (previousHighscore.CurrentHighscore < currentLevel.CurrentHighscore)
            newHighscore = true;
        else if (!previousHighscore.gameWon && currentLevel.gameWon)
            newHighscore = true;

        return newHighscore;
    }

    #region Gameplay

    //refactored to use tileID instead of view as only the id is necessary
    private void OnTileClicked(int tileID)
    {
        if (currentSelectedTile == null)
        {
            SelectTile(tileID);
        }
        else if (tileID != currentSelectedTile.TileID)
        {
            Tile targetTile = allTiles[tileID];
            if (PathCalculator.IsPathAvailable(currentSelectedTile, targetTile))
            {
                ScoreCorrectMatch(targetTile);
                CheckWinLoseConditions();
            }
            else
            {
                ScoreIncorrectMatch();
            }
        }
        else
        {
            DeselectTile();
        }
    }

    private void CheckWinLoseConditions()
    {
        if (interactableTiles.Count == 0)
        {
            currentLevel.gameWon = true;
            UiView.ShowGameSummary(true, currentLevel.CurrentHighscore);
        }
        else
        {
            if (!IsHintValid())
            {
                currentHint = PathCalculator.GetValidPathPair(interactableTiles);
                if (currentHint == null)
                {
                    UiView.ShowGameSummary(false, currentLevel.CurrentHighscore);
                }
            }
        }
    }

    private void ScoreIncorrectMatch()
    {
        currentLevel.CurrentHighscore = Mathf.Max(0, currentLevel.CurrentHighscore - ScoreLossPerFail);
        currentSelectedTile.Deselect();
        currentSelectedTile = null;
        UiView.SetCurrentScore(currentLevel.CurrentHighscore);
    }

    private void ScoreCorrectMatch(Tile targetTile)
    {
        currentLevel.CurrentHighscore += ScorePerMatch;
        currentSelectedTile.RemoveTile();
        targetTile.RemoveTile();
        interactableTiles.Remove(currentSelectedTile);
        interactableTiles.Remove(targetTile);
        currentSelectedTile = null;
        UiView.SetCurrentScore(currentLevel.CurrentHighscore);
    }

    private void SelectTile(int tileID)
    {
        currentSelectedTile = allTiles[tileID];
        currentSelectedTile.Select();
    }

    private void DeselectTile()
    {
        currentSelectedTile.Deselect();
        currentSelectedTile = null;
    }

    private bool IsHintValid()
    {
        return currentHint[0].TileFace == currentHint[1].TileFace && currentHint[0].TileFace != TileFaces.Empty;
    }
    #endregion
}