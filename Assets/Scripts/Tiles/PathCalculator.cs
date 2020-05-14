using System.Collections.Generic;

public static class PathCalculator
{
    public static bool IsPathAvailable(Tile tile1, Tile tile2)
    {
        Directions firstDirection = Directions.Top;

        foreach (var neighbor in tile1.neighbors)
        {
            firstDirection = neighbor.Key;
            break;
        }

        return IsPathComplete(tile1, tile2, tile1, firstDirection, 0);
    }

    public static Tile[] GetValidPathPair(List<Tile> interactableTiles)
    {
        Tile[] result = null;

        for (int i = 0; i < interactableTiles.Count; i++)
        {
            Tile startTile = interactableTiles[i];
            var validTargets = interactableTiles.FindAll((tile) => tile.TileFace == startTile.TileFace);
            validTargets.Remove(startTile);

            for (int k = 0; k < validTargets.Count; k++)
            {
                if (IsPathAvailable(startTile, validTargets[k]))
                {
                    result = new Tile[] { startTile, validTargets[k] };
                    break;
                }
            }
            if (result != null)
                break;
        }

        return result;
    }

    private static bool IsPathComplete(Tile start, Tile end, Tile current, Directions curDirection, int curvesDone)
    {
        bool result = false;

        if (curvesDone <= 2)
        {
            if (start.TileFace == end.TileFace)
            {
                if (current.TileID == end.TileID)
                    return true;
                if (current.TileFace == TileFaces.Empty || current == start)
                {
                    foreach (var neighbor in current.neighbors)
                    {
                        bool recursivePathComplete = false;
                        if (neighbor.Key == curDirection || current == start)
                        {
                            recursivePathComplete = IsPathComplete(start, end, neighbor.Value, neighbor.Key, curvesDone);
                        }
                        else
                        {
                            recursivePathComplete = IsPathComplete(start, end, neighbor.Value, neighbor.Key, curvesDone + 1);
                        }
                        if (recursivePathComplete)
                        {
                            result = true;
                            break;
                        }
                    }
                }
            }
        }

        return result;
    }
}