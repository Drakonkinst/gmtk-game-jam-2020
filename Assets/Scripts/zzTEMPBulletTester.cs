using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zzTEMPBulletTester : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public Transform parentObject;
    private Transform gunTransform;
    private Transform myTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        myTransform = transform;
        gunTransform = GameObject.FindWithTag("Gun").transform;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            GameObject bullet = Instantiate(prefabToSpawn, gunTransform);
            bullet.transform.position = gunTransform.position;
        }
    }

}
