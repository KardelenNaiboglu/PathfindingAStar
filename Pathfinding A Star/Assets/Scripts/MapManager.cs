using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum TileType
{
    Empty,
    Obstacle,
    Source,
    Destination
}

public class MapManager : MonoBehaviour
{
    public int rowCount = 5;
    public int columnCount = 5;
    public Tile tilePrefab;
    public float distanceBetweenEachTile = 1.1f;

    private List<Tile> _tiles;

    private TileType _selectedTileType = TileType.Empty;

    //==========================================================================

    private void Start()
    {
        GenerateMap();
    }

    //==========================================================================

    private void OnEnable()
    {
        EventsManager.Instance.OnTileClicked += OnTileClicked;
        EventsManager.Instance.OnTileTypeSelected += OnTileTypeSelected;
    }

    //==========================================================================

    private void OnDisable()
    {
        EventsManager.Instance.OnTileClicked -= OnTileClicked;
        EventsManager.Instance.OnTileTypeSelected -= OnTileTypeSelected;
    }

    //==========================================================================

    private void OnTileClicked(Tile tile)
    {
        switch (_selectedTileType)
        {
            case TileType.Source:
                Tile sourceTile = _tiles.Where(t => t.GetTileType() == TileType.Source).FirstOrDefault();

                if(sourceTile != null)
                {
                    sourceTile.SetTile(TileType.Empty);
                }

                break;

            case TileType.Destination:

                Tile destinationTile = _tiles.Where(t => t.GetTileType() == TileType.Destination).FirstOrDefault();

                if (destinationTile != null)
                {
                    destinationTile.SetTile(TileType.Empty);
                }
                break;

            default:
                break;
        }

        tile.SetTile(_selectedTileType);
    }

    //==========================================================================

    private void OnTileTypeSelected(TileType tileType)
    {
        _selectedTileType = tileType;
    }

    //==========================================================================

    private void GenerateMap()
    {
        Tile tile;

        _tiles = new();

        for (int i = 0; i < rowCount; i++)
        {
            for (int j = 0; j < columnCount; j++)
            {
                tile = Instantiate(tilePrefab);
                tile.transform.position =
                    new Vector3(distanceBetweenEachTile * (j - (columnCount - 1) / 2f), distanceBetweenEachTile * (i - (rowCount - 1) / 2f), 0f);
                _tiles.Add(tile);
            }
        }
    }

    //==========================================================================

}
