using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridPCG : MonoBehaviour
{
    public Grid grid = null;
    public GridSquare squarePrefab = null;
    public GameObject playerPrefab = null;
    public float gridSize = 1;
    public int gridHeight = 10;
    public int gridWidth = 10;
    public bool generateRandomSeed = true;
    public System.Random random = null;
    public System.Random seedGenerator = null;
    public int seed = 0;

    public List<Vector3Int> directions = new List<Vector3Int>() {
        new Vector3Int(1, 0, 0),
        new Vector3Int(0, 0, 1),
        new Vector3Int(-1, 0, 0),
        new Vector3Int(0, 0, -1)
    };

    public Vector3Int start = Vector3Int.zero;

    public void CreateRandomMaze(int root)
    {
        int totalSquares = gridHeight * gridWidth;
        List<int> next = new List<int>();
        List<bool> inTree = new List<bool>();

        for (int i = 0; i < totalSquares; i++)
        {
            inTree.Add(false);
            next.Add(-1);
        }
        next[root] = -1;
        inTree[root] = true;
        for (int i = 1; i < totalSquares; i++)
        {
            int u = i;
            while (!inTree[u])
            {
                next[u] = GetRandomSuccessor(u);
                u = next[u];
            }
            u = i;
            while (!inTree[u])
            {
                inTree[u] = true;
                u = next[u];
            }
        }
        RemoveWalls(totalSquares, next);
    }

    public void RemoveWalls(int totalSquares, List<int> next)
    {
        int nextID = -1;
        for (int i = 1; i < next.Count; i++)
        {
            GridSquare temp = grid.AllSquares[i];
            nextID = next[i];
            temp.nextSquareID = nextID;
            grid.AllSquares[nextID].VisitorIds.Add(temp.GetID());
        }

        for (int i = 1; i < totalSquares; i++)
        {
            if (grid.AllSquares[i].Position.z < grid.AllSquares[grid.AllSquares[i].nextSquareID].Position.z)
            {
                grid.AllSquares[i].walls[0].SetActive(false);
                grid.AllSquares[grid.AllSquares[i].nextSquareID].walls[2].SetActive(false);
            }
            else if (grid.AllSquares[i].Position.z > grid.AllSquares[grid.AllSquares[i].nextSquareID].Position.z)
            {
                grid.AllSquares[i].walls[2].SetActive(false);
                grid.AllSquares[grid.AllSquares[i].nextSquareID].walls[0].SetActive(false);
            }
            else if (grid.AllSquares[i].Position.x < grid.AllSquares[grid.AllSquares[i].nextSquareID].Position.x)
            {
                grid.AllSquares[i].walls[1].SetActive(false);
                grid.AllSquares[grid.AllSquares[i].nextSquareID].walls[3].SetActive(false);
            }
            else if (grid.AllSquares[i].Position.x > grid.AllSquares[grid.AllSquares[i].nextSquareID].Position.x)
            {
                grid.AllSquares[i].walls[3].SetActive(false);
                grid.AllSquares[grid.AllSquares[i].nextSquareID].walls[1].SetActive(false);
            }
        }

        SpawnRooms(totalSquares);

        grid.transform.localScale *= gridSize;

        SpawnPlayer();
    }

    public int GetRandomSuccessor(int index)
    {
        GridSquare currentSquare = grid.AllSquares[index];

        return currentSquare.Neighbors[random.Next(0, currentSquare.Neighbors.Count)].GetID();
    }

    public void SpawnRooms(int totalSquares)
    {
        int numRoomsToSpawn = random.Next(0, (int)Math.Ceiling(totalSquares * .025));

        for(int i = 0; i < numRoomsToSpawn; i++)
        {
            GridSquare startingSpot = grid.AllSquares[random.Next(0, grid.AllSquares.Count)];
            int roomSize = random.Next(3, (int)Math.Ceiling(totalSquares/numRoomsToSpawn * 0.01) > 3 ? (int)Math.Ceiling(totalSquares / numRoomsToSpawn * 0.01) : 4);
            HashSet<GridSquare> cellsInRoom = new HashSet<GridSquare>();
            cellsInRoom = GetRoomCells(startingSpot, roomSize);
            foreach(var cell in cellsInRoom)
            {
                cell.RemoveAllWalls();
            }
        }
    }

    HashSet<GridSquare> GetRoomCells(GridSquare currCell, int remainingSize)
    {
        HashSet<GridSquare> cells = new HashSet<GridSquare>();
        currCell.includedInRoom = true;
        foreach(var neighbor in currCell.Neighbors)
        {
            if(remainingSize > 0)
            {
                if(!neighbor.includedInRoom)
                {
                    cells.UnionWith(GetRoomCells(neighbor, remainingSize - 1));
                }
                else
                {
                    cells.Add(neighbor);
                }
            }
            else
            {
                cells.Add(neighbor);
            }
        }
        return cells;
    }

    public void Build()
    {
        if (grid == null)
        {
            grid = CreateGrid();
        }

        if (random == null)
        {
            random = CreateRng();
            grid.name = $"Dungeon Grid {seed}";
        }
        int curID = 0;
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                GridSquare sq = CreateSquare(new Vector3Int(i, 0, j), curID);
                sq.transform.position = grid.BoardToWorld(new Vector3Int(i, 0, j));
                sq.gameObject.SetActive(true);
                curID++;
            }
        }
        CreateRandomMaze(0);
    }

    //IEnumerator BuildDungeon()
    //{
    //    long curID = 0;
    //    for (int i = 0; i < gridWidth; i++)
    //    {
    //        for (int j = 0; j < gridHeight; j++)
    //        {
    //            GridSquare sq = CreateSquare(new Vector3Int(i, 0, j), curID);
    //            sq.transform.position = grid.BoardToWorld(new Vector3Int(i, 0, j));
    //            sq.gameObject.SetActive(true);
    //            curID++;
    //        }
    //        yield return null;
    //    }
    //    CreateRandomMaze(0);
    //}

    public void Clear()
    {
        if (grid != null)
        {
            DestroyImmediate(grid.gameObject);
        }
    }

    public void SpawnPlayer()
    {
        Vector3 newPos = new Vector3(grid.AllSquares[0].transform.localScale.x / 2, grid.AllSquares[0].transform.localScale.y + gridSize, grid.AllSquares[0].transform.localScale.z / 2);
        GameObject player = Instantiate(playerPrefab, grid.AllSquares[grid.AllSquares.Count - 1].Position + newPos, Quaternion.identity, grid.transform);
        player.transform.localScale = player.transform.localScale / 2;
    }

    public GridSquare CreateSquare(Vector3Int pos, int curID)
    {
        GridSquare sq = Instantiate(squarePrefab, grid.transform);
        sq.Position = pos;
        sq.SetID(curID);
        grid.AddSquare(sq);
        return sq;
    }

    public Grid CreateGrid()
    {
        GameObject gridObj = new GameObject("Dungeon Grid");
        Grid g = gridObj.AddComponent<Grid>();
        return g;
    }

    public System.Random CreateRng()
    {
        if (generateRandomSeed)
        {
            if (seedGenerator == null)
                seed = (int)Time.time * 1000;
            else
                seed = seedGenerator.Next();
        }

        random = new System.Random(seed);
        return random;
    }
}
