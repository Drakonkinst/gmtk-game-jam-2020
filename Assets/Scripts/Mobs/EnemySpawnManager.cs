using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public static EnemySpawnManager Instance;
    public Transform enemyParent;
    public float spawnInterval = 2.0f;
    public float minSpawnDistance = 5.0f;
    public GameObject enemyType1;
    public GameObject enemyType2;
    public int maxEnemies = 10;
    public List<GameObject> enemies = new List<GameObject>();
    public float timeTillMoreEnemies = 10.0f;
    public int enemyCountIncrease = 5;
    
    private int numEnemies;
    private Transform player;
    
    void Start() {
        Instance = this;
        numEnemies = 0;
        player = SceneManager.Instance.playerTransform;
        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnMoreEnemies());
    }
    
    private IEnumerator SpawnMoreEnemies() {
        while(true) {
            yield return new WaitForSeconds(timeTillMoreEnemies);
            maxEnemies += enemyCountIncrease;
        }
    }
    
    private IEnumerator SpawnEnemies() {
        while(true) {
            yield return new WaitForSeconds(spawnInterval);
            if(numEnemies < maxEnemies) {
                SpawnRandomEnemy();
            }
        }
    }
    
    public void SpawnRandomEnemy() {
        Vector3 spawnPos = FindRandomEnemySpawn();
        if(spawnPos == Vector3.zero) {
            return;
        }
        GameObject enemyToSpawn = enemyType1;
        int choice = Random.Range(0, 2);
        if(choice == 0) {
            enemyToSpawn = enemyType1;
        } else if(choice == 1) {
            enemyToSpawn = enemyType2;
        }
        GameObject enemySpawned = Instantiate(enemyToSpawn, enemyParent);
        enemies.Add(enemySpawned);
        CleanList();
        enemySpawned.transform.position = spawnPos;
        numEnemies++;
    }
    
    private void CleanList() {
        foreach(GameObject enemy in new List<GameObject>(enemies)) {
            if(enemy == null) {
                enemies.Remove(enemy);
            }
        }
    }
    private Vector3 FindRandomEnemySpawn() {
        return SceneManager.Instance.GetPointAwayFromPlayer(minSpawnDistance, 1.0f);
    }
}
