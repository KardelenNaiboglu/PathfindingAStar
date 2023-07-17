using System;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class EventsManager : MonoBehaviour
{
    public static EventsManager Instance { get; private set; }

    public delegate void OnTileClickedDelegate(Tile tile);
    public event OnTileClickedDelegate OnTileClicked;

    public delegate void OnTileTypeSelectedDelegate(TileType tileType);
    public event OnTileTypeSelectedDelegate OnTileTypeSelected;

    public delegate void OnButtonClickedFindPathDelegate();
    public event OnButtonClickedFindPathDelegate OnButtonClickedFindPath;

    //==========================================================================

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    //==========================================================================

    public void ClickedTile(Tile tile)
    {
        OnTileClicked?.Invoke(tile);
    }

    //==========================================================================

    public void ButtonClickedTileTypeSelection(string enumString)
    {
        if(Enum.TryParse(typeof(TileType), enumString, out object result))
        {
            OnTileTypeSelected?.Invoke((TileType)result);
        }
    }

    //==========================================================================

    public void ButtonClickedFindPath()
    {
        OnButtonClickedFindPath?.Invoke();
    }

    //==========================================================================
}
