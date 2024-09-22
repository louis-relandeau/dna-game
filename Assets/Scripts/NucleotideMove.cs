using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NucleotideMoveScript : MonoBehaviour, IRelativeTime
{
    [System.NonSerialized]
    public float frequency = 1.05f,
                 amplitude = 1.35f,
                 phaseShift = 2f; // Appears as x offset

    public float relativeTime { get; set; } = 1f;

    public float xPos;

    public void SetStartPos(float currTime) {
        xPos = Mathf.Sin(currTime * frequency + phaseShift) * amplitude;
    }

    void Update()
    {
        float y = transform.position.y - Time.deltaTime * relativeTime;
        transform.position = new Vector3(xPos, y, transform.position.z);
    }
}
