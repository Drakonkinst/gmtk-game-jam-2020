using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelManager : MonoBehaviour
{
    public int healthMax = 4;
    public int health;
    public float timeToBlink = 0.1f;
    private bool damaged = false;
    private bool started = false;
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
            Debug.Log("Barrel Health: " + health);
            CheckStatus();
        }
    }

    void CheckStatus() {

        if (health <= 0)
        {
            //explode();
            Destroy(gameObject);
        }
        if (health - 2 <= 0 && !started) {
            started = true;
            StartCoroutine("BlinkRed");
        }
    }

    IEnumerator BlinkRed() {
        Debug.Log("Entered Coroutine.");
        while((timeToBlink -= 0.05f) >= 0f) {
            yield return new WaitForSeconds(timeToBlink);
            damaged = !damaged;
            if(!damaged) {
                rend.material = damagedState;
                Debug.Log("Changed to Magma.");
            }
            else {
                rend.material = defaultState;
                Debug.Log("Changed to Wooden.");
            }
        }
        //explode();
        Destroy(gameObject);
    }

    /*
    void explode() {
        
    }
    */
}
