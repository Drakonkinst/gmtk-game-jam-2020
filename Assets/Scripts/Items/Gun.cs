using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public static Gun activeGun;
    public static List<Gun> guns = new List<Gun>();
    public GameObject bulletPrefab;
    public float fireRate = 0.5f;
    public KeyCode button = KeyCode.Mouse0;
    private GameObject player;
    [System.NonSerialized]
    public Transform myTransform;
    
    private Transform bulletParent;
    private float nextFire;
    // Start is called before the first frame update
    void Awake() {
        guns.Add(this);
    }
    
    void Start()
    {
        bulletParent = SceneManager.Instance.bulletParent;
        myTransform = transform;
        nextFire = Time.time;
        player = SceneManager.Instance.player;
        if(this is Pistol) {
            //Debug.Log("Found pistol!");
            SetThisToActiveGun();
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGunPosition();
        if(Input.GetKey(button))
        {
            float currTime = Time.time;
            if(currTime > nextFire) {
                nextFire = currTime + fireRate;
                bool fired = Fire();
                if(fired) {
                    SetThisToActiveGun();
                }
            }
        }
    }
    
    private void SetThisToActiveGun() {
        activeGun = this;
        SetRenderer(true);
        for(int i = 0; i < guns.Count; i++) {
            if(guns[i] != this) {
                guns[i].SetRenderer(false);
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
        
        if(knockBack > 0) {
            player.GetComponent<Rigidbody>().AddForce(myTransform.forward * -knockBack * 50.0f, ForceMode.Impulse); // Adds a force to the chicken opposite to the rotation of the gun 
        }
        return bullet;
    }
    
    private IEnumerator BulletExpire(GameObject bullet, float seconds) {
        yield return new WaitForSeconds(seconds);
        if(bullet != null) {
            Destroy(bullet);
        }
    }
    
    public virtual void SetRenderer(bool flag) {}
    
    public virtual bool Fire() {
        // I refuse to do anything >:) Go implement your own firing code buddy
        return false;
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
