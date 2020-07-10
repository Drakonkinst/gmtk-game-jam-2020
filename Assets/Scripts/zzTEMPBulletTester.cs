using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zzTEMPBulletTester : MonoBehaviour
{
    public GameObject prefabToSpawn;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(prefabToSpawn, transform);
        }
    }

}
