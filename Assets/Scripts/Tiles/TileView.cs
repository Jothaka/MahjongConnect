using UnityEngine;
using UnityEngine.UI;

public class TileView : MonoBehaviour
{
    private static readonly int HintTriggerHash = Animator.StringToHash("Hint");

    public delegate void OnTileInteraction(int id);

    public event OnTileInteraction OnTileClicked;

    public TileData Data { get; private set; }

    [SerializeField]
    private Image tileImage;
    [SerializeField]
    private Button viewButton;
    [SerializeField]
    private Animator buttonAnimator;

    public int TileID { get; private set; }

    public void SetTileID(int id)
    {
        TileID = id;
    }

    public void SetTileData(TileData data)
    {
        Data = data;
        tileImage.sprite = data.tileSprite;
    }

    public void OnClicked()
    {
        OnTileClicked?.Invoke(TileID);
    }

    public void Select()
    {
        viewButton.Select();
    }

    public void Highlight()
    {
        buttonAnimator.SetTrigger(HintTriggerHash);
    }
}