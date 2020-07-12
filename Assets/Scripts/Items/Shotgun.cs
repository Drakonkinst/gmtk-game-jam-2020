using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Gun
{
    public float bulletSpeed = 50.0f;
    public float inaccuracy = 10.0f;
    public float distance = 1.0f;
    public float height = 0.20f;
    public float lifetime = 0.5f;
    public float knockBack = 0.5f;
    public float fleeTime = 0.2f;

    public override bool Fire() {
        if(SceneManager.Instance.IsShotgunDisabled || SceneManager.Instance.currentShotgunAmmo <= 0) {
            return false;
        }
        Quaternion leftMost = myTransform.rotation * Quaternion.Euler(0f, -inaccuracy, 0f); 
        Quaternion rightMost = myTransform.rotation * Quaternion.Euler(0f, inaccuracy, 0f); 
        SpawnBullet(distance, height, leftMost, bulletSpeed, lifetime, knockBack);  // Creates a bullet at the maximum inaccuracy on
        SpawnBullet(distance, height, rightMost, bulletSpeed, lifetime, knockBack); // both sides of the shotgun barrel
        SpawnBullet(distance, height, myTransform.rotation * Quaternion.Euler(0f, UnityEngine.Random.Range(-inaccuracy,inaccuracy), 0f), bulletSpeed, lifetime, knockBack); // Creates three randomly inaccurate bullets within the same inaccuracy cone.
        SpawnBullet(distance, height, myTransform.rotation * Quaternion.Euler(0f, UnityEngine.Random.Range(-inaccuracy, inaccuracy), 0f), bulletSpeed, lifetime, knockBack);
        SpawnBullet(distance, height, myTransform.rotation * Quaternion.Euler(0f, UnityEngine.Random.Range(-inaccuracy, inaccuracy), 0f), bulletSpeed, lifetime, knockBack);
        PlayerMovement playerMvt = SceneManager.Instance.player.GetComponent<PlayerMovement>();
        Vector3 fleeFrom = myTransform.forward + myTransform.position;
        playerMvt.Flee(fleeFrom, fleeTime);
        SceneManager.Instance.OnShotgunFire();
        return true;
        //Debug.Log("Fleeing from: " + fleeFrom);
    }
    
    public override void SetRenderer(bool flag) {
        Transform model = transform.Find("Shotgun");
        if(model == null) {
            return;
        }
        model.GetComponent<MeshRenderer>().enabled = flag;
        model.Find("Pump_Loader").GetComponent<MeshRenderer>().enabled = flag;
        
    }
}
