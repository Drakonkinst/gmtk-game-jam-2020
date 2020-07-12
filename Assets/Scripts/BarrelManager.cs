using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BarrelManager : MonoBehaviour
{
    public int healthMax = 4;
    public int health;
    public float timeToBlink = 0.5f;
    public float explosionDuration = 3.0f;
    public float explosionRadius = 10.0f;
    private bool damaged = false;
    private bool started = false;
    private Renderer rend;
    public Material defaultState;
    public Material damagedState;
    public Texture barrelTexture;
    public Material invisible;
    public GameObject child;
    private ParticleSystem explosion;
    private GameObject[] barrelsInRadius;
    private Transform myTransform;
    private LineRenderer line;
    public int circleSegments = 50;
    public float lineWidth = 0.1f;
    public int mobDamage = 5;
    public float playerDamage = 20.0f;
    public AudioClip explosionSound;
    
    // Start is called before the first frame update
    void Start()
    {
        myTransform = transform;
        rend = GetComponent<Renderer>();
        rend.material.mainTexture = barrelTexture;
        health = healthMax;
        explosion = Instantiate(child, myTransform).GetComponent<ParticleSystem>();
        line = GetComponent<LineRenderer>();
        line.SetVertexCount(circleSegments + 1);
        line.useWorldSpace = false;
        line.SetWidth(lineWidth, lineWidth);
        CreatePoints();
    }
    
    void OnTriggerEnter(Collider col) {
        if(col.gameObject.tag == "Bullet") { // Barrel takes damage when struck by bullet
            health--;
            CheckStatus();
        }
    }
    
    private void CreatePoints ()
    {
        float x;
        float y;
        float z;

        float angle = 20f;

        for (int i = 0; i < (circleSegments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * explosionRadius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * explosionRadius;

            line.SetPosition(i,new Vector3(x,-1,y) );

            angle += (360f / circleSegments);
        }
    }

    public void SetHealth(int hp)
    {
        health = hp;
    }

    public void LightFuse()
    {
        StartCoroutine("BlinkRed");
    }

    void CheckStatus() {

        if (health <= 0)
        {
            Explode();
        }
        if (health - 2 <= 0 && !started) {
            StartCoroutine("BlinkRed");
        }
    }

    IEnumerator BlinkRed() {
        started = true;
        while((timeToBlink -= 0.1f) >= 0f) {
            yield return new WaitForSeconds(timeToBlink);
            if(damaged) {
                rend.material = defaultState;
                rend.material.mainTexture = barrelTexture;
            }
            else {
                rend.material = damagedState;
                rend.material.mainTexture = barrelTexture;
            }
            damaged = !damaged;
        }
        StartCoroutine("Explode");
    }

    IEnumerator Explode() {
        barrelsInRadius = GameObject.FindGameObjectsWithTag("Barrel");
        SoundManager.Instance.Play(explosionSound, SceneManager.Instance.camera.transform);
        rend.material = invisible;
        explosion.GetComponent<ParticleSystem>().Play();
        foreach(GameObject barrel in barrelsInRadius)
        {
            float distance = Vector3.Distance(myTransform.position, barrel.transform.position);
            if (barrel != this && (distance <= explosionRadius))
            {
                BarrelManager bm = barrel.GetComponent<BarrelManager>();
                if(bm.damaged == false)
                {
                    bm.damaged = true;
                    bm.SetHealth(2);
                    bm.LightFuse();
                }
                
            }
        }
        foreach(GameObject enemy in EnemySpawnManager.Instance.enemies) {
            if(enemy == null) {
                continue;
            }
            if(Vector3.Distance(myTransform.position, enemy.transform.position) <= explosionRadius) {
                enemy.GetComponent<Breakable>().Damage(mobDamage);
            }
        }
        if(Vector3.Distance(myTransform.position, SceneManager.Instance.playerTransform.position) <= explosionRadius) {
            Debug.Log("Explosion hit player!");
            SceneManager.Instance.DamagePlayer(playerDamage);
        }
        yield return new WaitForSeconds(explosionDuration);
        Destroy(gameObject);
    }
    
}
