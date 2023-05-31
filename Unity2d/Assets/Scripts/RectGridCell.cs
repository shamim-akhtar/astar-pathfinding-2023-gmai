using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectGridCell : MonoBehaviour
{
  [SerializeField]
  SpriteRenderer outerSprite;
  [SerializeField]
  SpriteRenderer innerSprite;

  public bool isWalkable = true;
  public Vector2Int index = Vector2Int.zero;

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

  public void SetInnerColor(Color color)
  {
    innerSprite.color = color;
  }

  public void SetOuterColor(Color color)
  {
    outerSprite.color = color;
  }
}
