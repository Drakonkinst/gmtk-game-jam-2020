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
    private bool damaged = false;
    private bool started = false;
    private Renderer rend;
    public Material defaultState;
    public Material damagedState;
    public Material invisible;
    public GameObject child;
    private ParticleSystem explosion;
    private float explosionDuration = 3.0f;
    private float explosionRadius = 50.0f;
    private GameObject[] barrelsInRadius;
    
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material = defaultState;
        health = healthMax;
        explosion = Instantiate(child, this.transform).GetComponent<ParticleSystem>();
    }
    
    void OnTriggerEnter(Collider col) {
        if(col.gameObject.tag == "Bullet") { // Barrel takes damage when struck by bullet
            health--;
            Debug.Log("Barrel Health: " + health);
            CheckStatus();
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
            started = true;
            StartCoroutine("BlinkRed");
        }
    }

    IEnumerator BlinkRed() {
        Debug.Log("Entered Coroutine.");
        while((timeToBlink -= 0.1f) >= 0f) {
            yield return new WaitForSeconds(timeToBlink);
            damaged = !damaged;
            if(damaged) {
                rend.material = damagedState;
                Debug.Log("Changed to Magma.");
            }
            else {
                rend.material = defaultState;
                Debug.Log("Changed to Wooden.");
            }
        }
        StartCoroutine("Explode");
    }

    IEnumerator Explode() {
        barrelsInRadius = GameObject.FindGameObjectsWithTag("Barrel");
        rend.material = invisible;
        explosion.GetComponent<ParticleSystem>().Play();
        Debug.Log("Barrels within range: " + barrelsInRadius);
        foreach(GameObject barrel in barrelsInRadius)
        {
            float distance = Vector3.Distance(this.transform.position, barrel.transform.position);
            Debug.Log("Distance: " + distance);
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
        yield return new WaitForSeconds(explosionDuration);
        Destroy(gameObject);
    }
    
}
