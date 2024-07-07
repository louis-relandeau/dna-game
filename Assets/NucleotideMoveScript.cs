using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NucleotideMoveScript : MonoBehaviour
{
    public float relativeTime = 1f;
    public float frequency = 1.2f;
    public float amplitude = 1.3f;
    public float phaseShift = 2f; // Appears as x offset

    private float elapsedTime = 0f;

    void Update()
    {
        elapsedTime += Time.deltaTime;
        float x = Mathf.Sin(elapsedTime * relativeTime * frequency + phaseShift) * amplitude;
        float y = transform.position.y - Time.deltaTime * relativeTime;
        transform.position = new Vector3(x, y, transform.position.z);
    }
}
