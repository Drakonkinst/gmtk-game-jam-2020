using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magma : MonoBehaviour
{
    public float tickRate = 0.5f;
    public float damage = 1.0f;
    public float distance = 2.5f;
    
    private Transform myTransform;
    private float minX;
    private float maxX;
    private float minZ;
    private float maxZ;
    private Transform playerTransform;
    // Start is called before the first frame update
    void Start()
    {
        myTransform = transform;
        playerTransform = SceneManager.Instance.playerTransform;
        Vector3 pos = myTransform.position;
        minX = pos.x - distance;
        maxX = pos.x + distance;
        minZ = pos.z - distance;
        maxZ = pos.z + distance;
        
        StartCoroutine(Tick());
    }
    
    private IEnumerator Tick() {
        while(true) {
            yield return new WaitForSeconds(tickRate);
            // check enemies
            foreach(GameObject enemy in EnemySpawnManager.Instance.enemies) {
                if(enemy == null) {
                    continue;
                }
                if(IsWithin(enemy.transform.position.x, enemy.transform.position.z)) {
                    enemy.GetComponent<Breakable>().Damage();
                }
            }
            // check player
            if(IsWithin(playerTransform.position.x, playerTransform.position.z)) {
                SceneManager.Instance.DamagePlayer(damage);
            }
        }
    }
    
    private bool IsWithin(float x, float z) {
        return x >= minX && x <= maxX && z >= minZ && z <= maxZ;
    }
}
