﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    private Transform myTransform;

    void Start()
    {
        myTransform = transform;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Math.Abs(myTransform.position.x) > 50.0f || Math.Abs(myTransform.position.z) > 50.0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if(!(col.gameObject.tag == "Gun" || col.gameObject.tag == "Player" || col.gameObject.tag == "Bullet" || col.gameObject.tag == "Ignore"))
        {
            Debug.Log("Bullet Collision with " + col.gameObject.name);
            Destroy(gameObject); // When I collide with something, destroy myself
            if(col.gameObject.GetComponent<Breakable>() != null) {
                col.gameObject.GetComponent<Breakable>().Damage(1);
            }
        } 
    }
}