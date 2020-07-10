using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform following;
    public float offset = 6;
    public float height = 10;
    
    private Transform myTransform;
    // Start is called before the first frame update
    void Start()
    {
        myTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        myTransform.position = following.position + new Vector3(-offset, height, -offset);
    }
}
