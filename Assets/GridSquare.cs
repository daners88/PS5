using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Quadrant
{
    FirstQuadrant = 1,
    SecondQuadrant = 2,
    ThirdQuadrant = 3,
    FourthQuadrant = 4
}

public class GridSquare : MonoBehaviour
{
    [SerializeField] private Vector3Int position = Vector3Int.zero;
    public int nextSquareID = -1;
    [SerializeField]
    private int square_id = -1;
    public bool includedInRoom = false;
    public bool gatherTrigger = false;
    public Quadrant quadrant;
    [SerializeField]
    public List<GameObject> walls = null;
    [SerializeField]
    public List<Door> doors = null;
    public List<int> VisitorIds = new List<int>();
    public Vector3Int Position
    {
        get { return position; }
        set { position = value; }
    }

    public void RemoveAllWalls(int h, int w)
    {
        for(int i = 0; i < walls.Count; i++)
        {
            CheckRemoveWall(i, h, w);
        }
    }

    public void PlaceDoor(GridSquare nextSq)
    {
        if (quadrant == Quadrant.FirstQuadrant && nextSq.quadrant == Quadrant.SecondQuadrant)
        {
            LockDoor(1, KeyRequired.Quad1toQuad2);
        }
        else if (quadrant == Quadrant.SecondQuadrant && nextSq.quadrant == Quadrant.FirstQuadrant)
        {
            LockDoor(3, KeyRequired.Quad1toQuad2);
        }
        else if (quadrant == Quadrant.FirstQuadrant && nextSq.quadrant == Quadrant.ThirdQuadrant)
        {
            LockDoor(0, KeyRequired.Quad1toQuad3);
        }
        else if (quadrant == Quadrant.ThirdQuadrant && nextSq.quadrant == Quadrant.FirstQuadrant)
        {
            LockDoor(2, KeyRequired.Quad1toQuad3);
        }
        else if (quadrant == Quadrant.SecondQuadrant && nextSq.quadrant == Quadrant.FourthQuadrant)
        {
            LockDoor(0, KeyRequired.Quad2toQuad4);
        }
        else if (quadrant == Quadrant.FourthQuadrant && nextSq.quadrant == Quadrant.SecondQuadrant)
        {
            LockDoor(2, KeyRequired.Quad2toQuad4);
        }
        else if (quadrant == Quadrant.ThirdQuadrant && nextSq.quadrant == Quadrant.FourthQuadrant)
        {
            LockDoor(1, KeyRequired.Quad3toQuad4);
        }
        else if (quadrant == Quadrant.FourthQuadrant && nextSq.quadrant == Quadrant.ThirdQuadrant)
        {
            LockDoor(3, KeyRequired.Quad3toQuad4);
        }
    }

    public void LockDoor(int doorID, KeyRequired kr)
    {
        doors[doorID].gameObject.SetActive(true);
        doors[doorID].keyReq = kr;
        doors[doorID].SetMat();
    }

    public void CheckRemoveWall(int wallID, int h, int w)
    {
        switch (wallID)
        {
            case 0:
                if (position.z < h - 1)
                {
                    walls[wallID].SetActive(false);
                }
                break;
            case 1:
                if (position.x < w - 1)
                {
                    walls[wallID].SetActive(false);
                }
                break;
            case 2:
                if (position.z > 0)
                {
                    walls[wallID].SetActive(false);
                }
                break;
            case 3:
                if (position.x > 0)
                {
                    walls[wallID].SetActive(false);
                }
                break;
            default:
                break;
        }
    }

    public void SetID(int id)
    {
        square_id = id;
    }

    public int GetID()
    {
        return square_id;
    }

    [SerializeField] private List<GridSquare> neighbors = null;
    public List<GridSquare> Neighbors
    {
        get { return neighbors; }
    }
    private Dictionary<Vector3Int, GridSquare> neighborsByDirection = null;

    public GridSquare GetNeighbor(Vector3Int dir)
    {
        if (neighborsByDirection.ContainsKey(dir))
            return neighborsByDirection[dir];
        else
            return null;
    }

    public void AddNeighbor(GridSquare neighbor)
    {
        if (neighbors.Contains(neighbor))
            return;

        neighbors.Add(neighbor);
        CacheNeighborMap();
        Vector3Int dir = CalcDirectionToNeighbor(neighbor);
        neighborsByDirection[dir] = neighbor;
    }

    private void CacheNeighborMap(bool hardReset = false)
    {
        if (neighborsByDirection == null || hardReset)
        {
            neighborsByDirection = new Dictionary<Vector3Int, GridSquare>();
            foreach (GridSquare neighbor in neighbors)
            {
                Vector3Int dir = CalcDirectionToNeighbor(neighbor);
                neighborsByDirection.Add(dir, neighbor);
            }
        }
    }

    private Vector3Int CalcDirectionToNeighbor(GridSquare neighbor)
    {
        Vector3Int dir = neighbor.Position - position;
        return dir;
    }
}
