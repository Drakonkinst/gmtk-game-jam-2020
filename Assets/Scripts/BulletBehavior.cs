using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    public float speed = 1;
    private Rigidbody myRb;
    // Start is called before the first frame update
    void Start()
    {
        myRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        myRb.AddRelativeForce(Vector3.forward * 250.0f * speed * Time.deltaTime, ForceMode.Force);
        if(Math.Abs((transform.position.x + transform.position.z)/2) > 50.0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if(!(col.gameObject.tag == "Gun" && col.gameObject.tag == "Player" && col.gameObject.tag == "Bullet"))
        {
            Debug.Log("Bullet Collision with " + col.gameObject.name);
            //Destroy(gameObject); // When I collide with something, destroy myself
        } 
    }
}
