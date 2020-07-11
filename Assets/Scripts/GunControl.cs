using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunControl : MonoBehaviour
{
    public Transform gun;
    
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        Vector3 lookPos = new Vector3();
        
        if(Physics.Raycast(ray, out hitInfo, Mathf.Infinity)) {
            Debug.Log("Hit");
            lookPos = hitInfo.point;
        } else {
            Debug.Log("No hit");
        }
        
        Vector3 lookDir = lookPos - gun.position;
        lookDir.y = 0;
        gun.LookAt(gun.position + lookDir);
    }
}
