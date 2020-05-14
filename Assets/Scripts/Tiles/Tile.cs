using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Tile
{
    public int TileID { get; private set; }
    public TileFaces TileFace { get; set; }
    public TileView View { get; set; }

    public Dictionary<Directions, Tile> neighbors = new Dictionary<Directions, Tile>();

    public Tile(int tileID, TileFaces face)
    {
        TileID = tileID;
        TileFace = face;
    }

    public void RemoveTile()
    {
        UnityEngine.Object.Destroy(View.gameObject);
        View = null;
        TileFace = TileFaces.Empty;
    }

    public void Select()
    {
        View.Select();
    }

    public void Deselect()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void Highlight()
    {
        View.Highlight();
    }
}