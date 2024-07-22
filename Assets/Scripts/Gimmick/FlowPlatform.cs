using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowPlatform : MonoBehaviour
{
    public enum Direction { Up, Down }

    [SerializeField] private Direction direction;
    [SerializeField] private float moveDistance;
    [SerializeField] private float moveSpeed;

    private Vector3 startPosition;
    private Vector3 goalPosition;
    void Start()
    {
        startPosition = transform.position;
        if (direction == Direction.Up)
        {
            goalPosition = new Vector3(transform.position.x, transform.position.y + moveDistance, transform.position.z);
            StartCoroutine(UpPlatformMoving(startPosition, goalPosition));
        }
        else
        {
            goalPosition = new Vector3(transform.position.x, transform.position.y - moveDistance, transform.position.z);
            StartCoroutine(DownPlatformMoving(startPosition, goalPosition));
        }
    }

    IEnumerator UpPlatformMoving(Vector3 start, Vector3 goal)
    {
        while (Vector3.Distance(transform.position, goal) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, goal, moveSpeed * Time.deltaTime);
            yield return null;
        }
        StartCoroutine(DownPlatformMoving(goal, start));
    }
    IEnumerator DownPlatformMoving(Vector3 start, Vector3 goal)
    {
        while (Vector3.Distance(transform.position, goal) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, goal, moveSpeed * Time.deltaTime);
            yield return null;
        }
        StartCoroutine(UpPlatformMoving(goal, start));
    }
}
