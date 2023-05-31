using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class RectGrid : MonoBehaviour
{
  public int mX = 20; // maximum number of columns
  public int mY = 20; // maximum number of rows.
  public GameObject rectGridCellPrefab;

  GameObject[,] cells = null;

  public Color COLOR_WALKABLE = Color.cyan;
  public Color COLOR_NON_WALKABLE = Color.black;
  public Color COLOR_PATH = Color.green;

  public bool noDiagonalMovement = false;

  // The NPC
  public NPCMovement npc;
  private GMAI.PathFinder pathFinder = new GMAI.PathFinder();

  // Start is called before the first frame update
  void Start()
  {
    cells = new GameObject[mX, mY];
    for(int i = 0; i < mX; ++i)
    {
      for(int j = 0; j < mY; ++j)
      {
        Vector2Int index = new Vector2Int(i, j);
        cells[i, j] = Instantiate(rectGridCellPrefab, new Vector3(i, j, 0.0f), Quaternion.identity);
        cells[i, j].transform.SetParent(transform, false);

        RectGridCell gridCell = cells[i,j].GetComponent<RectGridCell>();
        if(gridCell != null)
        {
          gridCell.index = index;
          gridCell.SetInnerColor(COLOR_WALKABLE);
        }

        // lets set the name of the grid cell.
        cells[i,j].name = "cell_" + i + "_" + j;
      }
    }

    pathFinder.heuristicCost = ManhattanCost;
    pathFinder.traversalCost = EuclideanCost;
    pathFinder.getNeighbours = GetNeighbours;
  }

  // Update is called once per frame
  void Update()
  {
    if(Input.GetMouseButtonDown(0))
    {
      ToggleWalkable();
    }

    if(Input.GetMouseButtonDown(1))
    {
      SetNPCDestination();
    }
  }

  void SetNPCDestination()
  {
    // Non pathfinding region
    // Ray cast to check which cell is intersected.
    Vector2 rayPos = new Vector2(
      Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
      Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
    RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0.0f);
    if (hit)
    {
      GameObject obj = hit.transform.gameObject;
      RectGridCell gridCell = obj.GetComponent<RectGridCell>();
      if (gridCell != null && npc != null)
      {
        npc.SetDestination(gridCell.index, this, pathFinder);
      }
    }
  }

  void ToggleWalkable()
  {
    // Ray cast to check which cell is intersected.
    Vector2 rayPos = new Vector2(
      Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
      Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
    RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0.0f);
    if(hit)
    {
      GameObject obj = hit.transform.gameObject;
      RectGridCell gridCell = obj.GetComponent<RectGridCell>();
      if(gridCell != null) 
      {
        ToggleWalkable(gridCell);
      }
    }
  }
  void ToggleWalkable(RectGridCell gridCell)
  {
    if(gridCell.isWalkable)
    {
      gridCell.SetInnerColor(COLOR_NON_WALKABLE);
      gridCell.isWalkable = false;
    }
    else
    {
      gridCell.SetInnerColor(COLOR_WALKABLE);
      gridCell.isWalkable = true;
    }
  }

  public void SetCellColor(Vector2Int cellIndex, Color color)
  {
    GameObject cell = cells[cellIndex.x, cellIndex.y];
    RectGridCell gridCell = cell.GetComponent<RectGridCell>();
    gridCell.SetInnerColor(color);
  }

  public void ResetCellColors()
  {
    for(int i = 0; i < mX; i++)
    {
      for(int j = 0; j < mY; j++)
      {
        GameObject cell = cells[i, j];
        RectGridCell gridCell = cell.GetComponent<RectGridCell>();
        if (!gridCell.isWalkable)
        {
          gridCell.SetInnerColor(COLOR_NON_WALKABLE);
        }
        else
        {
          gridCell.SetInnerColor(COLOR_WALKABLE);
        }

      }

    }
  }

  // Delegate implementation
  public float ManhattanCost(Vector2Int a, Vector2Int b)
  {
    return Mathf.Abs(a.x-b.x) + Mathf.Abs(a.y-b.y);
  }
  public float EuclideanCost(Vector2Int a, Vector2Int b)
  {
    return Vector2Int.Distance(a, b);
  }

  public List<Vector2Int> GetNeighbours(Vector2Int a)
  {
    List<Vector2Int> neighbours = new List<Vector2Int>();

    int x = a.x;
    int y = a.y;

    // Check up
    if(y < mY - 1)
    {
      int i = x;
      int j = y + 1;

      RectGridCell gridCell = cells[i, j].GetComponent<RectGridCell>();
      if (gridCell.isWalkable)
      {
        // add to the list of neighbours.
        neighbours.Add(gridCell.index);
      }
    }
    if (!noDiagonalMovement)
    {
      // Check top-right
      if (y < mY - 1 && x < mX - 1)
      {
        int i = x + 1;
        int j = y + 1;

        RectGridCell gridCell = cells[i, j].GetComponent<RectGridCell>();
        if (gridCell.isWalkable)
        {
          // add to the list of neighbours.
          neighbours.Add(gridCell.index);
        }
      }
    }
    // Check right
    if (x < mX - 1)
    {
      int i = x + 1;
      int j = y;

      RectGridCell gridCell = cells[i, j].GetComponent<RectGridCell>();
      if (gridCell.isWalkable)
      {
        // add to the list of neighbours.
        neighbours.Add(gridCell.index);
      }
    }
    if (!noDiagonalMovement)
    {
      // Check right-down
      if (x < mX - 1 && y > 0)
      {
        int i = x + 1;
        int j = y - 1;

        RectGridCell gridCell = cells[i, j].GetComponent<RectGridCell>();
        if (gridCell.isWalkable)
        {
          // add to the list of neighbours.
          neighbours.Add(gridCell.index);
        }
      }
    }
    // Check down
    if ( y > 0)
    {
      int i = x;
      int j = y - 1;

      RectGridCell gridCell = cells[i, j].GetComponent<RectGridCell>();
      if (gridCell.isWalkable)
      {
        // add to the list of neighbours.
        neighbours.Add(gridCell.index);
      }
    }
    if (!noDiagonalMovement)
    {
      // Check down-left
      if (y > 0 && x > 0)
      {
        int i = x - 1;
        int j = y - 1;

        RectGridCell gridCell = cells[i, j].GetComponent<RectGridCell>();
        if (gridCell.isWalkable)
        {
          // add to the list of neighbours.
          neighbours.Add(gridCell.index);
        }
      }
    }
    // Check left
    if (x > 0)
    {
      int i = x - 1;
      int j = y;

      RectGridCell gridCell = cells[i, j].GetComponent<RectGridCell>();
      if (gridCell.isWalkable)
      {
        // add to the list of neighbours.
        neighbours.Add(gridCell.index);
      }
    }
    if (!noDiagonalMovement)
    {
      // Check left-top
      if (x > 0 && y < mY - 1)
      {
        int i = x - 1;
        int j = y + 1;

        RectGridCell gridCell = cells[i, j].GetComponent<RectGridCell>();
        if (gridCell.isWalkable)
        {
          // add to the list of neighbours.
          neighbours.Add(gridCell.index);
        }
      }
    }
    return neighbours;
  }
}
