using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private LevelButtonView[] buttonViews;

    private void Start()
    {
        List<Level> allLevels = HighscoreUtility.LoadAllHighscores();

        for (int i = 0; i < allLevels.Count; i++)
        {
            var level = allLevels[i];
            var view = buttonViews.First((element) => element.LevelID == level.LevelID);
            view.SetHighscore(level.CurrentHighscore.ToString(), level.gameWon);
        }
    }

    //triggered by UI-Event
    public void OnLevelButtonClicked(int levelID)
    {
        GameManager.Instance.LevelID = levelID;
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}