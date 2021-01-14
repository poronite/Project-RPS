using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    public GameObject WaterObject;

    public float Speed;
    private Vector2 spawnPosition = new Vector2(-18.28f, 6.5f);
    private Vector2 finalPosition = new Vector2(40, 6.5f);

    private void Update()
    {
        float flow = Speed * Time.deltaTime;

        transform.position = Vector2.MoveTowards(transform.position, finalPosition, Speed);

        if (transform.position.x == 40)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("WaterSpawner"))
        {
            Instantiate(WaterObject, spawnPosition, Quaternion.identity);
            Debug.Log("aaaaaaaa");
        }
    }
}
