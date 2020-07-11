using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelManager : MonoBehaviour
{
    public int healthMax = 4;
    public int health;
    public float timeToBlink = 0.5f;
    private bool damaged = false;
    private Renderer rend;
    public Material defaultState;
    public Material damagedState;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material = defaultState;
        health = healthMax;
    }
    
    void OnTriggerEnter(Collider col) {
        if(col.gameObject.tag == "Bullet") { // Barrel takes damage when struck by bullet
            health--;
            Debug.Log(health);
        }
        checkStatus();
    }

    void checkStatus() {
        if(health - 2 <= 0) {
            damaged = true;
            StartCoroutine("blinkRed");
        }
        else if (health <= 0) {
            //explode();
            damaged = false;
            Destroy(gameObject);
        }
    }

    IEnumerator blinkRed() {
        while(damaged) {
            yield return new WaitForSeconds(timeToBlink * 1000.0f);
            if(rend.material == defaultState) {
                rend.material = damagedState;
            }
            else {
                rend.material = defaultState;
            }
        }
    }

    /*
    void explode() {
        
    }
    */
}
