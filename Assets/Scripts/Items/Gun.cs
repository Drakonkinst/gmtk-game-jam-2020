using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float fireRate = 0.5f;
    private Rigidbody playerRb;
    [System.NonSerialized]
    public Transform myTransform;
    
    private Transform bulletParent;
    private float nextFire;
    // Start is called before the first frame update
    void Start()
    {
        bulletParent = SceneManager.Instance.bulletParent;
        myTransform = transform;
        nextFire = Time.time;
        playerRb = GameObject.FindWithTag("Player").GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGunPosition();
        if(Input.GetKey(KeyCode.Mouse0))
        {
            float currTime = Time.time;
            if(currTime > nextFire) {
                nextFire = currTime + fireRate;
                Fire();
            }
        }
    }
    
    public GameObject SpawnBullet(float distance, float height, Quaternion rotation, float speed, float lifetime, float knockBack) {
        float angle = myTransform.eulerAngles.y * Mathf.Deg2Rad;
        float offsetX = Mathf.Sin(angle) * distance;
        float offsetZ = Mathf.Cos(angle) * distance;
        
        GameObject bullet = Instantiate(bulletPrefab, myTransform.position + new Vector3(offsetX, height, offsetZ), rotation, bulletParent);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if(rb != null) {
            rb.AddRelativeForce(Vector3.forward * speed, ForceMode.Impulse);
        }
        StartCoroutine(BulletExpire(bullet, lifetime));
        playerRb.AddForce(this.transform.forward * -knockBack * 50.0f,ForceMode.Impulse); // Adds a force to the chicken opposite to the rotation of the gun
        return bullet;
    }
    
    private IEnumerator BulletExpire(GameObject bullet, float seconds) {
        yield return new WaitForSeconds(seconds);
        if(bullet != null) {
            Destroy(bullet);;
        }
    }
    
    public virtual void Fire() {
        // I refuse to do anything >:) Go implement your own firing code buddy
    }
    
    
    private void UpdateGunPosition() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        
        if(Physics.Raycast(ray, out hitInfo, Mathf.Infinity)) {
            Vector3 lookDir = hitInfo.point - myTransform.position;
            lookDir.y = 0;
            myTransform.LookAt(myTransform.position + lookDir);
        } else {
            Debug.LogWarning("Raycast failed!");
        }
    }
}
