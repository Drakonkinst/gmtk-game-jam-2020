using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Gun
{
    public float bulletSpeed = 10.0f;
    public float inaccuracy = 5.0f;
    public Vector3 offset = new Vector3(0.0f, 0.25f, 0.0f);

    public override void Fire()
    {
        // spawns bullet at object with random rotation
        SpawnBullet(myTransform.position + offset, myTransform.rotation * Quaternion.Euler(0.0f, Random.Range(-inaccuracy,inaccuracy), 0.0f), bulletSpeed);
    }
}