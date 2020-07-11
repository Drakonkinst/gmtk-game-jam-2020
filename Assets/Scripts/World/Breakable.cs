using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    public int health = 3;
    
    // return health after
    public int Damage(int howMuch = 1) {
        Debug.Log("Damaged!");
        health -= howMuch;
        if(health <= 0) {
            Destroy(gameObject);
            return 0;
        }
        return health;
    }
}
