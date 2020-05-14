using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

public class GameCreatorController : MonoBehaviour
{
    public delegate void OnCreationFinishedDelegate(List<Tile> levelLayout);
    public event OnCreationFinishedDelegate OnCreationFinished;

    private const char TILE = 'X';
    private const char EMPTY = 'O';

    [Header("References")]
    [SerializeField]
    private Transform GridParent;

    [SerializeField]
    private TextAsset[] levels;

    [SerializeField]
    private TileView viewPrefab;

    [SerializeField]
    private TileData[] tileDatas;

    [Header("Settings")]
    public Vector2 GridStartPosition;
    public Vector2 TileSize;
    public Vector2 Padding;

    private List<Tile> allTiles = new List<Tile>();

    public void CreateLevel(int levelID)
    {
        allTiles.Clear();

        var levelAsset = levels[levelID];

        List<string> cleanedLines = ParseLevelAsset(levelAsset);

        int height = cleanedLines.Count;
        Tile[][] levelLayout = new Tile[height][];

        Vector2 spawnCursor = GridStartPosition;
        int tileID = 0;

        for (int i = 0; i < cleanedLines.Count; i++)
        {
            string line = cleanedLines[i];
            levelLayout[i] = new Tile[line.Length];
            for (int k = 0; k < line.Length; k++)
            {
                char t = line[k];

                Tile currentTile = new Tile(tileID, TileFaces.Empty);
                levelLayout[i][k] = currentTile;
                if (t == TILE)
                {
                    var spawnedTileView = Instantiate(viewPrefab, spawnCursor, Quaternion.identity, GridParent);
                    RectTransform spawnRect = spawnedTileView.transform as RectTransform;
                    spawnRect.anchoredPosition = spawnCursor;

                    spawnedTileView.SetTileData(tileDatas[UnityRandom.Range(0, tileDatas.Length)]);
                    currentTile.TileFace = spawnedTileView.Data.tileFace;
                    spawnedTileView.SetTileID(currentTile.TileID);
                    currentTile.View = spawnedTileView;
                }
                spawnCursor.x += TileSize.x + Padding.x;
                allTiles.Add(currentTile);
                tileID++;
            }

            spawnCursor.y -= TileSize.y + Padding.y;
            spawnCursor.x = GridStartPosition.x;
        }

        AssignNeighbors(levelLayout);

        OnCreationFinished?.Invoke(allTiles);
    }

    private static List<string> ParseLevelAsset(TextAsset levelAsset)
    {
        string completeLevel = levelAsset.text;
        string[] lines = Regex.Split(completeLevel, "\n|\r|\r\n|\t");

        List<string> cleanedLines = new List<string>();
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].Length > 1)
                cleanedLines.Add(lines[i]);
        }

        List<string> paddedLines = PaddLines(cleanedLines);

        return paddedLines;
    }

    //add a layer of empty tiles around the whole game so the corner tiles are accessible
    private static List<string> PaddLines(List<string> cleanedLines)
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(EMPTY, cleanedLines[0].Length + 2);

        List<string> paddedLines = new List<string>();
        paddedLines.Add(stringBuilder.ToString());

        StringBuilder paddedLine = new StringBuilder();
        for (int i = 0; i < cleanedLines.Count; i++)
        {
            paddedLine.Clear();
            paddedLine.Append(EMPTY);
            paddedLine.Append(cleanedLines[i]);
            paddedLine.Append(EMPTY);
            paddedLines.Add(paddedLine.ToString());
        }

        paddedLines.Add(stringBuilder.ToString());
        return paddedLines;
    }

    private void AssignNeighbors(Tile[][] levelLayout)
    {
        for (int height = 0; height < levelLayout.Length; height++)
        {
            for (int width = 0; width < levelLayout[height].Length; width++)
            {
                Tile currentTile = levelLayout[height][width];
                if (width > 0)
                    currentTile.neighbors.Add(Directions.Left, levelLayout[height][width - 1]);
                if (width < levelLayout[height].Length - 1)
                    currentTile.neighbors.Add(Directions.Right, levelLayout[height][width + 1]);
                if (height > 0)
                    currentTile.neighbors.Add(Directions.Top, levelLayout[height - 1][width]);
                if (height < levelLayout.Length - 1)
                    currentTile.neighbors.Add(Directions.Down, levelLayout[height + 1][width]);
            }
        }
    }
}