using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    public float fireRate = 0.5f;
    public Transform myTransform;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10.0f;
    public float inaccuracy = 10.0f;
    public float distance = 0.75f;
    public float height = 0.20f;
    public float lifetime = 5.0f;
    public float shotDelay = 1.0f;
    public Transform bulletParent;
    public AudioClip fireSound;


    private float nextFire;
    // Start is called before the first frame update
    void Start()
    {
        bulletParent = SceneManager.Instance.bulletParent;
    }

    private IEnumerator BulletExpire(GameObject bullet, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (bullet != null)
        {
            Destroy(bullet);
        }
    }

    public void Shoot()
    {
        SpawnBullet(distance, height, myTransform.rotation, bulletSpeed, lifetime);
        SoundManager.Instance.Play(fireSound, SceneManager.Instance.camera.transform);
    }

    public GameObject SpawnBullet(float distance, float height, Quaternion rotation, float speed, float lifetime)
    {
        float angle = myTransform.eulerAngles.y * Mathf.Deg2Rad;
        float offsetX = Mathf.Sin(angle) * distance;
        float offsetZ = Mathf.Cos(angle) * distance;

        GameObject bullet = Instantiate(bulletPrefab, myTransform.position + new Vector3(offsetX, height, offsetZ), rotation, bulletParent);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddRelativeForce(Vector3.forward * speed, ForceMode.Impulse);
        }
        StartCoroutine(BulletExpire(bullet, lifetime));

        return bullet;
    }
}
