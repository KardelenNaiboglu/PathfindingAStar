using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private Renderer _renderer;
    private TileType _tileType;
    private int _index;
    private int _gValue;
    private int _hValue;
    private Tile _parentTile;

    //==========================================================================

    private void Awake()
    {
        _renderer = GetComponentInChildren<Renderer>();
        Reset();
    }

    //==========================================================================

    private void Reset()
    {
        SetTile(TileType.Empty);
    }

    //==========================================================================

    private void SetColor()
    {
        switch (_tileType)
        {
            case TileType.Empty:
                _renderer.material.color = Color.white;
                break;

            case TileType.Obstacle:
                _renderer.material.color = Color.black;
                break;

            case TileType.Source:
                _renderer.material.color = Color.green;
                break;

            case TileType.Destination:
                _renderer.material.color = Color.red;
                break;
        }
    }

    //==========================================================================

    public void Highlight()
    {
        _renderer.material.color = Color.cyan;
    }

    //==========================================================================

    public void SetIndex(int index)
    {
        _index = index;
    }

    //==========================================================================

    public int GetIndex()
    {
        return _index;
    }

    //==========================================================================

    public void SetTile(TileType tileType)
    {
        _tileType = tileType;

        SetColor();
    }

    //==========================================================================

    public TileType GetTileType()
    {
        return _tileType;
    }

    //==========================================================================

    public int GetFValue()
    {
        return _gValue + _hValue;
    }

    //==========================================================================

    public void SetGValue(int gVal)
    {
        _gValue = gVal;
    }

    //==========================================================================

    public int GetGValue()
    {
        return _gValue;
    }

    //==========================================================================

    public void SetHValue(int hVal)
    {
        _hValue = hVal;
    }

    //==========================================================================

    public int GetHValue()
    {
        return _hValue;
    }

    //==========================================================================

    public void SetParent(Tile parent)
    {
        _parentTile = parent;
    }

    //==========================================================================

    public Tile GetParent()
    {
        return _parentTile;
    }
}
