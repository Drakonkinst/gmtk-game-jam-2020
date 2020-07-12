﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    public static EnemySpawnManager Instance;
    public Transform enemyParent;
    public float spawnInterval = 3.0f;
    public float minSpawnDistance = 5.0f;
    public int maxAttempts = 10;
    public GameObject enemyType1;
    public GameObject enemyType2;
    public int maxEnemies = 10;
    public List<GameObject> enemies = new List<GameObject>();
    
    private int numEnemies;
    private Transform player;
    
    void Start() {
        Instance = this;
        numEnemies = 0;
        player = SceneManager.Instance.playerTransform;
        StartCoroutine(SpawnEnemies());
    }
    
    private IEnumerator SpawnEnemies() {
        while(true) {
            yield return new WaitForSeconds(spawnInterval);
            if(numEnemies < maxEnemies) {
                SpawnRandomEnemy();
            }
        }
    }
    
    private void SpawnRandomEnemy() {
        Vector3 spawnPos = FindRandomEnemySpawn();
        if(spawnPos == Vector3.zero) {
            return;
        }
        GameObject enemyToSpawn = enemyType1;
        
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
        Vector3 spawnPoint;
        int numAttempts = 0;
        do {
            Vector2 randomPt = SceneManager.Instance.GetRandomWorldPoint();
            spawnPoint = new Vector3(randomPt.x, 1.0f, randomPt.y);
            numAttempts++;
        } while(numAttempts < maxAttempts && Vector3.Distance(spawnPoint, player.position) < minSpawnDistance);
        if(numAttempts == maxAttempts) {
            Debug.Log("Failed to spawn!");
            return Vector3.zero;
        }
        return spawnPoint;
    }
}
