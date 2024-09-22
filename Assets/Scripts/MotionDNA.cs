using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionDNA : MonoBehaviour, IRelativeTime
{
    public float relativeTime { get; set; } = 1f;
    
    void Update()
    {
        float y = transform.position.y - Time.deltaTime * relativeTime;
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }
}
