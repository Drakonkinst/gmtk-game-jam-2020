using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zzTEMPBulletTester : MonoBehaviour
{
    public GameObject prefabToSpawn;
    //public Transform parentObject;
    public float inaccuracy = 5.0f;
    private Transform myTransform;

    
    // Start is called before the first frame update
    void Start()
    {
        myTransform = transform;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Instantiate(prefabToSpawn, myTransform.position + new Vector3(0.0f, 0.25f, 0.0f), myTransform.rotation * Quaternion.Euler(0f,Random.Range(-inaccuracy,inaccuracy),0f)); // spawns a bullet at the object and 
            //GameObject bullet = Instantiate(prefabToSpawn, parentObject);
            //bullet.transform.position = gunTransform.position;
        }
    }

}
