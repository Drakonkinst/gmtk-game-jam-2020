using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using UnityEngine;

public class GunController : MonoBehaviour
{
    private Transform playerTransform;
    private Transform myTransform;
    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        myTransform = transform;
        offset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        setPosition();
    }

    void setPosition() {
        transform.position = playerTransform.position + new Vector3(0,1.5f,0);
    }
}
