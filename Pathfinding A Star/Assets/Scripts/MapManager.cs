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
        EventsManager.Instance.OnButtonClickedFindPath += OnButtonClickedFindPath;
    }

    //==========================================================================

    private void OnDisable()
    {
        EventsManager.Instance.OnTileClicked -= OnTileClicked;
        EventsManager.Instance.OnTileTypeSelected -= OnTileTypeSelected;
        EventsManager.Instance.OnButtonClickedFindPath -= OnButtonClickedFindPath;
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

    private void OnButtonClickedFindPath()
    {
        FindPath();
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
                tile.SetIndex(_tiles.Count());
                _tiles.Add(tile);
            }
        }
    }

    //==========================================================================

    private void FindPath()
    {
        Tile sourceTile = _tiles.Where(t => t.GetTileType() == TileType.Source).FirstOrDefault();

        if (sourceTile == null)
        {
            Debug.LogError("Select a source tile!");
            return;
        }

        Tile destinationTile = _tiles.Where(t => t.GetTileType() == TileType.Destination).FirstOrDefault();

        if (destinationTile == null)
        {
            Debug.LogError("Select a destination tile!");
            return;
        }

        List<Tile> openList = new();
        List<Tile> closedList = new();

        openList.Add(sourceTile);
        sourceTile.SetHValue(0);
        destinationTile.SetHValue(0);

        while(openList.Count > 0)
        {
            Tile currentTile = openList.OrderBy(t => t.GetFValue()).FirstOrDefault();

            if(currentTile == destinationTile)
            {
                DrawPath();
                return;
            }

            openList.Remove(currentTile);
            closedList.Add(currentTile);

            List<Tile> adjacentTiles = GetAvailableAdjacentTiles(currentTile);

            foreach(Tile adjacentTile in adjacentTiles)
            {
                if (closedList.Contains(adjacentTile))
                {
                    continue;
                }

                if (!openList.Contains(adjacentTile))
                {

                    adjacentTile.SetGValue(currentTile.GetGValue() + 1);
                    adjacentTile.SetHValue(GetDistanceTo(adjacentTile, destinationTile));
                    adjacentTile.SetParent(currentTile);

                    openList.Add(adjacentTile);
                }
                else
                {
                    if (currentTile.GetGValue() + adjacentTile.GetHValue() + 1 < adjacentTile.GetFValue())
                    {
                        adjacentTile.SetGValue(currentTile.GetGValue() + 1);
                        adjacentTile.SetParent(currentTile);
                    }
                }
            }
        }
    }

    //==========================================================================

    private void DrawPath()
    {
        Tile sourceTile = _tiles.Where(t => t.GetTileType() == TileType.Source).FirstOrDefault();

        Tile destinationTile = _tiles.Where(t => t.GetTileType() == TileType.Destination).FirstOrDefault();
        Tile parentTile = destinationTile;

        while (true)
        {
            parentTile = parentTile.GetParent();

            if(parentTile == sourceTile)
            {
                break;
            }

            parentTile.Highlight();
        }
    }

    //==========================================================================

    private List<Tile> GetAvailableAdjacentTiles(Tile currentTile)
    {
        int currentIndex = currentTile.GetIndex();
        List<Tile> availableTiles = new();

        int[] tileIndexes = new int[]
        {
            currentIndex + 1, //right
            currentIndex - 1, //left
            currentIndex - rowCount, //up
            currentIndex - rowCount - 1, //up left
            currentIndex - rowCount + 1, //up right
            currentIndex + rowCount, //down
            currentIndex + rowCount - 1, //down left
            currentIndex + rowCount + 1 //down right
        };

        for(int i = 0; i < tileIndexes.Length; i++)
        {
            if (IsValidOrAvailable(tileIndexes[i]))
            {
                availableTiles.Add(_tiles[tileIndexes[i]]);
            }
        }

        return availableTiles;
    }

    //==========================================================================

    private bool IsValidOrAvailable(int index)
    {
        if(index < 0 || index >= _tiles.Count)
        {
            return false;
        }

        return _tiles[index].GetTileType() != TileType.Obstacle;
    }

    //==========================================================================

    private int GetDistanceTo(Tile currentTile, Tile destination)
    {
        int sourceIndex = currentTile.GetIndex();
        int destinationIndex = destination.GetIndex();
        return Mathf.Abs((sourceIndex - destinationIndex) / rowCount) + Mathf.Abs((sourceIndex-destinationIndex) % rowCount);
    }
}