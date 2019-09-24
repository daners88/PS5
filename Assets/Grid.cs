using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public static readonly List<Vector3Int> Directions = new List<Vector3Int>() {
        new Vector3Int(1, 0, 0),
        new Vector3Int(0, 1, 0),
        new Vector3Int(0, 0, 1),
        new Vector3Int(-1, 0, 0),
        new Vector3Int(0, -1, 0),
        new Vector3Int(0, 0, -1)
    };

    [SerializeField] private List<GridSquare> allSquares = new List<GridSquare>();
    public List<GridSquare> AllSquares
    {
        get { return allSquares; }
    }

    private Dictionary<Vector3Int, GridSquare> squaresByPosition = null;

    public GridSquare Get(Vector3Int pos)
    {
        CacheMaps();
        if (squaresByPosition.ContainsKey(pos))
            return squaresByPosition[pos];
        return null;
    }

    public GridSquare this[Vector3Int pos]
    {
        get { return Get(pos); }
    }

    private void CacheMaps()
    {
        if (squaresByPosition == null)
        {
            squaresByPosition = new Dictionary<Vector3Int, GridSquare>();
            foreach (GridSquare square in allSquares)
            {
                squaresByPosition[square.Position] = square;
            }
        }
    }

    public Vector3Int WorldToBoard(Vector3 w)
    {
        return new Vector3Int((int)w.x, (int)w.y, (int)w.z);
    }

    public Vector3 BoardToWorld(Vector3Int b)
    {
        return new Vector3(b.x, b.y, b.z);
    }

    public void AddSquare(GridSquare square)
    {
        CacheMaps();
        allSquares.Add(square);
        squaresByPosition[square.Position] = square;

        ConnectSquareToNeighbors(square);
    }

    private void ConnectSquareToNeighbors(GridSquare square)
    {
        foreach (Vector3Int dir in Directions)
        {
            Vector3Int nPos = square.Position + dir;
            GridSquare neighbor = Get(nPos);
            if (neighbor != null)
            {
                square.AddNeighbor(neighbor);
                neighbor.AddNeighbor(square);
            }
        }
    }
}
