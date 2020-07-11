using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : Steerable
{
    public Transform following;
    public float offset = 6;
    public float height = 10;
    public float slowingDist = 0.2f;
    
    public override void DoBehavior() {
        Vector3 seeking = following.position + new Vector3(-offset, height, -offset);
        steering.Seek(seeking, slowingDist);
    }
}