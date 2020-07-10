using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zzTEMPBulletTester : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public Transform parentObject;
    
    private Transform myTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        myTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GameObject bullet = Instantiate(prefabToSpawn, parentObject);
            bullet.transform.position = myTransform.position;
            
        }
    }

}
