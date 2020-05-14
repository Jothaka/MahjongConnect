using UnityEngine;
using System.IO;
using System.Collections.Generic;

public static class HighscoreUtility 
{
    public static List<Level> LoadAllHighscores()
    {
        var levels = new List<Level>();
        for (int i = 0; i < 5; i++)
        {
            Level lv = LoadSpecificHighscore(i);
            if (lv != null)
                levels.Add(lv);
        }
        return levels;
    }

    public static Level LoadSpecificHighscore(int levelID)
    {
        string pathString = string.Format("highscore_{0}.json", levelID);
        Level lv = null;
        if (File.Exists(pathString))
        {
            using (StreamReader wr = new StreamReader(pathString))
            {
                string jsonString = wr.ReadLine();
                lv = JsonUtility.FromJson<Level>(jsonString);
            }
        }
        return lv;
    }

    public static void SaveHighscore(Level levelToSave)
    {
        string jsonString = JsonUtility.ToJson(levelToSave);
        string pathString = string.Format("highscore_{0}.json", levelToSave.LevelID);
        using (StreamWriter wr = new StreamWriter(pathString))
        {
            wr.WriteLine(jsonString);
        }
    }
}