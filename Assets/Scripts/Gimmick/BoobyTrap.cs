using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BoobyTrap : MonoBehaviour
{
    public enum Direction { Up, Down, Right, Left }

    [SerializeField] private Direction direction;
    [SerializeField] private GameObject arrow;
    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private float arrowSpeed = 3f;
    [SerializeField] private Transform player;

    private Vector3 spawnDirection;
    void Start()
    {
        if (direction == Direction.Up) spawnDirection = Vector3.up;
        else if (direction == Direction.Down) spawnDirection = Vector3.down;
        else if (direction == Direction.Right) spawnDirection = Vector3.right;
        else spawnDirection = Vector3.left;
    }

    IEnumerator SpawnArrowRoutine()
    {
        while (true) { 
            GameObject spawnArraow = Instantiate(arrow, transform.position, Quaternion.identity);
            Rigidbody2D rigid = spawnArraow.GetComponent<Rigidbody2D>();
            if (rigid != null) rigid.velocity = spawnDirection.normalized * arrowSpeed;

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) StartCoroutine(SpawnArrowRoutine());
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) StopAllCoroutines();
    }
}
