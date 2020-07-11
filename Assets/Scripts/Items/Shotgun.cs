using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gun
{
    public float bulletSpeed = 50.0f;
    public float inaccuracy = 5.0f;
    public float distance = 1.0f;
    public float height = 0.20f;
    
    public override void Fire() {
        SpawnBullet(distance, height, myTransform.rotation, bulletSpeed);
    }
}
