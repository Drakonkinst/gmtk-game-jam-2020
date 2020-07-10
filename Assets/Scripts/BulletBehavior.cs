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
        myRb.AddForce(Vector3.forward * 250 * speed * Time.deltaTime, ForceMode.Force);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag != "Player")
        {
            Debug.Log("Bullet Collision with " + collision.gameObject.name);
            Destroy(gameObject); // When I collide with something, destroy myself
        } 
    }
}
