using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This rect grid is our map for pathfinding.

public class RectGrid : MonoBehaviour
{
  public int mX = 20;
  public int mY = 20;

  public GameObject rectGridCellPrefab;

  public Color COLOR_WALKABLE = Color.cyan;
  public Color COLOR_NON_WALKABLE = Color.black;

  // The 2d array of RectGridCell
  GameObject[,] cells = null;

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
        cells[i,j].name = "cell_" + i + "_" + j;
        cells[i,j].transform.SetParent(transform, false);

        RectGridCell rectGridCell = cells[i,j].GetComponent<RectGridCell>();
        if(rectGridCell != null)
        {
          rectGridCell.SetInnerColor(COLOR_WALKABLE);
        }
      }
    }
  }

  // Update is called once per frame
  void Update()
  {
    if(Input.GetMouseButtonDown(0))
    {
      ToggleWalkable();
    }
  }

  void ToggleWalkable()
  {
    Vector2 rayPos = new Vector2(
      Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
      Camera.main.ScreenToWorldPoint(Input.mousePosition).y);

    RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero, 0.0f);
    if(hit)
    {
      GameObject obj = hit.transform.gameObject;
      RectGridCell rectGridCell = obj.GetComponent<RectGridCell>();
      if(rectGridCell != null)
      {
        ToggleWalkable(rectGridCell);
      }
    }
  }

  void ToggleWalkable(RectGridCell rectGridCell)
  {
    if(!rectGridCell.isWalkable)
    {
      rectGridCell.SetInnerColor(COLOR_WALKABLE);
      rectGridCell.isWalkable = true;
    }
    else
    {
      rectGridCell.SetInnerColor(COLOR_NON_WALKABLE);
      rectGridCell.isWalkable = false;
    }
  }
}
