using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPMovement : MonoBehaviour
{
  public float speed = 1f;
  Queue<Vector2Int> wayPoints = new Queue<Vector2Int>();

  // Start is called before the first frame update
  void Start()
  {
    StartCoroutine(Coroutine_MoveTo());
  }

  // Update is called once per frame
  void Update()
  {

  }

  public void AddWayPoint(Vector2Int wayPoint)
  {
    wayPoints.Enqueue(wayPoint);
  }

  public void SetDestination(Vector2Int destination)
  {
    // We do not have pathfinding yet. So 
    // We will use this function just to set the waypoint.
    AddWayPoint(destination);

    // Later on when we implement the path finding we will modify this 
    // function to use the pathfinder and find the path to the destination.
    // TODO:
  }

  IEnumerator Coroutine_MoveTo()
  {
    while(true)
    {
      while(wayPoints.Count > 0)
      {
        yield return StartCoroutine(Coroutine_MoveToPoint(wayPoints.Dequeue()));
      }
      yield return null;
    }
  }

  IEnumerator Coroutine_MoveToPoint(Vector2Int p)
  {
    Vector2 startP = new Vector2(transform.position.x, transform.position.y);
    float distance = Vector2.Distance(startP, p);
    float totalTime = distance / speed;
    float elaspedTime = 0.0f;

    while(elaspedTime < totalTime)
    {
      float t = Mathf.Clamp01(elaspedTime/totalTime);
      Vector2 pos = Vector2.Lerp(startP, p, t);
      transform.position = new Vector3(pos.x, pos.y, transform.position.z);

      elaspedTime += Time.deltaTime;
      yield return new WaitForEndOfFrame();
    }
    transform.position = new Vector3(p.x, p.y, transform.position.z);
  }
}
