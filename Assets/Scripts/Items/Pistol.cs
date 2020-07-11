using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Gun
{
    public float bulletSpeed = 10.0f;
    public float inaccuracy = 5.0f;
    public float distance = 1.0f;
    public float height = 0.20f;
    public float lifetime = 5.0f;
    public float knockBack = 0.0f;

    public override bool Fire()
    {
        // spawns bullet at object with random rotation
        SpawnBullet(distance, height, myTransform.rotation * Quaternion.Euler(0.0f, Random.Range(-inaccuracy,inaccuracy), 0.0f), bulletSpeed, lifetime, knockBack);
        return true;
    }
    
    public override void SetRenderer(bool flag) {
        myTransform.Find("Barrel").GetComponent<MeshRenderer>().enabled = flag;
        myTransform.Find("Handle").GetComponent<MeshRenderer>().enabled = flag;
    }
}