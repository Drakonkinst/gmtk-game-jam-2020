﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance;
    
    public GameObject player;
    public GameObject ground;
    public GameObject gunPlane;
    public new GameObject camera;
    public GameObject healthBar;
    public GameObject ammoCount;
    public Transform bulletParent;
    
    public GameObject magmaHazard;
    public GameObject barrelHazard;
    public Transform hazardParent;
    
    public float maxHealth = 100.0f;
    public float currentHealth = 50.0f;
    public int maxShotgunAmmo = 6;
    public int currentShotgunAmmo = 6;
    public float magmaExpiryDecrement = 0.1f;
    public float magmaExpiryTime = 12.0f;
    public int maxSpawnAttempts = 10;
    public float hazardSpawnDistanceMin = 5.0f;
    public float hazardSpawnDistanceMax = 15.0f;
    public float barrelGroupDistance = 10.0f;
    public int numBarrelsInGroup = 3;
    public int initialHazards = 10;
    public float shotgunDisableTime = 8.0f;
    public float speedDuration = 4.0f;
    public float playerSpeedUp = 10.0f;
    public float cameraSpeedUp = 12.0f;
    public float playerSlowDown = 1.0f;
    public float playerHealthGain = 5.0f;
    public float reloadTime = 3.0f;
    public int numShellsOnReload = 1;
    public int numEnemyBoost = 10;
    public float damageSoundCooldown = 2.0f;
    public AudioClip damageSound;
    public AudioClip speedUpSound;
    public AudioClip slowDownSound;
    public float eventInterval = 4.0f;
    public float spawnMargin = 5.0f;
    
    public Vector2 worldCenter = new Vector2(0, 0);
    [System.NonSerialized]
    public Transform playerTransform;
    [System.NonSerialized]
    public Rigidbody playerRb;
    [System.NonSerialized]
    public float worldWidth;
    [System.NonSerialized]
    public float worldLength;
    [System.NonSerialized]
    public bool IsShotgunDisabled = false;
    
    private float healthBarWidth;
    private int score = 0;
    private float nextDamageSound;
    
    public void PlayDamageSound() {
        float currTime = Time.time;
        if(currTime > nextDamageSound) {
            SoundManager.Instance.Play(damageSound, camera.transform);
            nextDamageSound = currTime + damageSoundCooldown;
        }
    }
    
    private IEnumerator RandomEvent() {
        while(true) {
            yield return new WaitForSeconds(eventInterval);
            int choice = Random.Range(0, 6);
            if(choice == 0) {
                SpawnRandomMagma();
            } else if(choice == 1) {
                SpawnRandomMagma();
            } else if(choice == 2) {
                SpawnBarrelBunch();
            } else if(choice == 3) {
                SpawnEnemies();
            } else if(choice == 4) {
                SoundManager.Instance.Play(speedUpSound, camera.transform, 0.5f);
                SpeedUpPlayer();
            } else if(choice == 5) {
                SlowDownPlayer();
                SoundManager.Instance.Play(slowDownSound, camera.transform, 0.5f);
            }
        }
    }
    public void SpawnEnemies() {
        if(EnemySpawnManager.Instance == null) {
            return;
        }
        for(int i = 0; i < numEnemyBoost; i++) {
            EnemySpawnManager.Instance.SpawnRandomEnemy();
        }
    }
    
    private IEnumerator AmmoReload() {
        while(true) {
            yield return new WaitForSeconds(reloadTime);
            HealthGain();
            if(IsShotgunDisabled) {
                IsShotgunDisabled = false;
            } else {
                currentShotgunAmmo += numShellsOnReload;
                if(currentShotgunAmmo > maxShotgunAmmo) {
                    currentShotgunAmmo = maxShotgunAmmo;
                }
                UpdateUIShotgunAmmo();
            }
        }
    }
    
    public void HealthGain() {
        currentHealth += playerHealthGain;
        if(currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
        UpdateUIHealth();
    }
    
    public void SpeedUpPlayer() {
        float playerSpeed = player.GetComponent<SteeringManager>().maxVelocity;
        float cameraSpeed = camera.GetComponent<SteeringManager>().maxVelocity;
        player.GetComponent<SteeringManager>().maxVelocity = playerSpeedUp;
        camera.GetComponent<SteeringManager>().maxVelocity = cameraSpeedUp;
        StartCoroutine(RestoreOriginalSpeed(playerSpeed, cameraSpeed));
    }
    
    public void SlowDownPlayer() {
        float playerSpeed = player.GetComponent<SteeringManager>().maxVelocity;
        float cameraSpeed = camera.GetComponent<SteeringManager>().maxVelocity;
        player.GetComponent<SteeringManager>().maxVelocity = playerSlowDown;
        //camera.GetComponent<SteeringManager>().maxVelocity = cameraSpeedUp;
        StartCoroutine(RestoreOriginalSpeed(playerSpeed, cameraSpeed));
    }
    
    public void DisableShotgun() {
        if(IsShotgunDisabled) {
            return;
        }
        IsShotgunDisabled = true;
        StartCoroutine(EnableShotgun());
    }
    
    public void SpawnRandomHazard() {
        int hazard = Random.Range(0, 2);
        if(hazard == 0) {
            SpawnRandomMagma();
        } else if(hazard == 1) {
            SpawnBarrelBunch();
        }
    }
    
    public void SpawnRandomMagma() {
        Vector3 pos = GetPointAwayFromPlayer(hazardSpawnDistanceMin, 0.01f, hazardSpawnDistanceMax);
        GameObject magma = Instantiate(magmaHazard, hazardParent);
        magma.transform.position = pos;
        StartCoroutine(MagmaSpawn(magma));
        StartCoroutine(MagmaExpire(magma));
    }
    
    public void SpawnBarrelBunch() {
        Vector3 pos = GetPointAwayFromPlayer(hazardSpawnDistanceMin, 1.0f, hazardSpawnDistanceMax);
        for(int i = 0; i < numBarrelsInGroup; i++) {
            float angle = Random.Range(0.0f, Mathf.PI * 2.0f);
            float distance = Random.Range(0.0f, barrelGroupDistance);
            float xOffset = Mathf.Cos(angle) * distance;
            float zOffset = Mathf.Sin(angle) * distance;
            GameObject barrel = Instantiate(barrelHazard, hazardParent);
            barrel.transform.position = pos + new Vector3(xOffset, 0, zOffset);
        }
    }
    
    void Awake() {
        if(Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        Instance = this;
        
        playerTransform = player.transform;
        playerRb = player.GetComponent<Rigidbody>();
        Vector3 size = ground.GetComponent<MeshRenderer>().bounds.size;
        worldWidth = size.x;
        worldLength = size.z;
        Debug.Log(worldWidth + " x " + worldLength);
        healthBarWidth = healthBar.GetComponent<RectTransform>().sizeDelta.x;
        nextDamageSound = Time.time;
        UpdateUIHealth();
        UpdateUIShotgunAmmo();
        Debug.Log("Clearing guns");
        Gun.guns.Clear();
        Gun.activeGun = null;
    }
    
    void Start() {
        for(int i = 0; i < initialHazards; i++) {
            SpawnRandomHazard();
        }
        StartCoroutine(TrackScore());
        StartCoroutine(AmmoReload());
        StartCoroutine(RandomEvent());
        //UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
    
    public void DamagePlayer(float points) {
        currentHealth -= points;
        if(currentHealth < 0) {
            OnGameEnd();
            currentHealth = 0;
        }
        PlayDamageSound();
        UpdateUIHealth();
    }
    
    public void OnShotgunFire() {
        currentShotgunAmmo--;
        UpdateUIShotgunAmmo();
        if(currentShotgunAmmo == 0) {
            IsShotgunDisabled = true;
        }
    }
    
    private void OnGameEnd() {
        UnityEngine.SceneManagement.SceneManager.LoadScene("DeathScreen");
        PlayerPrefs.SetInt("player_score", score);
    }
    
    private void UpdateUIHealth() {
        healthBar.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, healthBarWidth * (currentHealth / maxHealth));
    }
    
    private void UpdateUIShotgunAmmo() {
        ammoCount.GetComponent<TextMeshProUGUI>().SetText("x " + currentShotgunAmmo);
    }
    
    private IEnumerator MagmaSpawn(GameObject magma) {
        magma.transform.localScale = new Vector3(0.1f, 1, 0.1f);
        while(magma.transform.localScale.x < 1) {
            yield return new WaitForSeconds(0.05f);
            magma.transform.localScale += new Vector3(magmaExpiryDecrement, 0, magmaExpiryDecrement);
        }
    }
    
    private IEnumerator MagmaExpire(GameObject magma) {
        yield return new WaitForSeconds(magmaExpiryTime);
        while(magma.transform.localScale.x > 0) {
            yield return new WaitForSeconds(0.05f);
            magma.transform.localScale -= new Vector3(magmaExpiryDecrement, 0, magmaExpiryDecrement);
        }
        AudioSource s = magma.GetComponent<Magma>().source;
        if(s != null) {
            s.Stop();
        }
        
        Destroy(magma);
    }
    
    private IEnumerator TrackScore() {
        while(true) {
            yield return new WaitForSeconds(1.0f);
            score++;
        }
    }
    
    private IEnumerator EnableShotgun() {
        yield return new WaitForSeconds(shotgunDisableTime);
        IsShotgunDisabled = false;
    }
    
    public Vector2 GetRandomWorldPoint() {
        float x = Random.Range(worldCenter.x - worldWidth / 2 + spawnMargin, worldCenter.x + worldWidth / 2 - spawnMargin);
        float y = Random.Range(worldCenter.y - worldLength / 2 + spawnMargin, worldCenter.y + worldLength / 2 - spawnMargin);
        return new Vector2(x, y);
    }
    
    public Vector3 GetPointAwayFromPlayer(float distance, float height, float maxDistance = Mathf.Infinity) {
        Vector3 spawnPoint;
        int numAttempts = 0;
        float d;
        do {
            Vector2 randomPt = SceneManager.Instance.GetRandomWorldPoint();
            spawnPoint = new Vector3(randomPt.x, height, randomPt.y);
            d = Vector3.Distance(spawnPoint, playerTransform.position);
            numAttempts++;
        } while(numAttempts < maxSpawnAttempts && d < distance && d < maxDistance);
        if(numAttempts == maxSpawnAttempts) {
            Debug.Log("Failed to spawn!");
            return Vector3.zero;
        }
        return spawnPoint;
    }
    
    private IEnumerator RestoreOriginalSpeed(float speed, float cameraSpeed) {
        yield return new WaitForSeconds(speedDuration);
        player.GetComponent<SteeringManager>().maxVelocity = speed;
        camera.GetComponent<SteeringManager>().maxVelocity = cameraSpeed;
    }
}