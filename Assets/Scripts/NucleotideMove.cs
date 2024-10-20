using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NucleotideMoveScript : MonoBehaviour, IRelativeTime
{
    [System.NonSerialized]    
    public float frequency = 1.25f,
                 amplitude = 1f,
                 phaseShift = 210 * Mathf.PI / 180,
                 xOffset = 0f;

    public float relativeTime { get; set; } = 1f;

    public float xPos;

    public void SetStartPosX(float currTime) {
        xPos = Mathf.Sin(currTime * frequency + phaseShift) * amplitude + xOffset;
    }

    void Update()
    {
        float y = transform.position.y - Time.deltaTime * relativeTime;
        transform.position = new Vector3(xPos, y, transform.position.z);
    }
}
