using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public float distance;
    public float rotation;
    public float speed;
    public Vector3 startingPos;
    Quaternion startingRot;
    float rand;
    void Start()
    {
        startingPos = transform.position;
        startingRot = transform.rotation;
        rand = Random.value * 1000f;
	}

    void Update()
    {
        Vector3 offset = new Vector2(Mathf.PerlinNoise1D(Time.time * speed + rand), Mathf.PerlinNoise1D(-Time.time * speed + rand));
        transform.position = startingPos + (offset - .5f * 2 * Vector3.one) * distance;
        transform.rotation = startingRot * Quaternion.Euler(0, 0, (offset.x - .5f) * 2 * rotation);
    }
}
