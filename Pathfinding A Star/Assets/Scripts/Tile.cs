using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private Renderer _renderer;
    private TileType _tileType;

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

}
