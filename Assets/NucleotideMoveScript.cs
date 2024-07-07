using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NucleotideMoveScript : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float frequency = 1.2f;
    public float amplitude = 1.3f;
    public float phaseShift = 2f; // Appears as x offset

    private float elapsedTime = 0f;

    void Update()
    {
        elapsedTime += Time.deltaTime;
        float x = Mathf.Sin(elapsedTime * frequency + phaseShift) * amplitude;
        float y = transform.position.y - (moveSpeed * Time.deltaTime);
        transform.position = new Vector3(x, y, transform.position.z);
    }
}
