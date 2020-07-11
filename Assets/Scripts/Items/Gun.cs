﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform spawnParent;
    public GameObject bulletPrefab;
    public float fireRate = 0.5f;
    
    [System.NonSerialized]
    public Transform myTransform;
    
    private float nextFire;
    // Start is called before the first frame update
    void Start()
    {
        myTransform = transform;
        nextFire = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGunPosition();
        if(Input.GetKey(KeyCode.Mouse0))
        {
            float currTime = Time.time;
            if(currTime > nextFire) {
                nextFire = currTime + fireRate;
                Fire();
            }
        }
    }
    
    public GameObject SpawnBullet(Vector3 position, Quaternion rotation, float speed) {
        GameObject bullet = Instantiate(bulletPrefab, position, rotation, spawnParent);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if(rb != null) {
            rb.AddRelativeForce(Vector3.forward * speed, ForceMode.Impulse);
        }
        return bullet;
    }
    
    public virtual void Fire() {
        // I refuse to do anything >:) Go implement your own firing code buddy
    }
    
    
    private void UpdateGunPosition() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        
        if(Physics.Raycast(ray, out hitInfo, Mathf.Infinity)) {
            Vector3 lookDir = hitInfo.point - myTransform.position;
            lookDir.y = 0;
            myTransform.LookAt(myTransform.position + lookDir);
        } else {
            Debug.LogWarning("Raycast failed!");
        }
    }
}