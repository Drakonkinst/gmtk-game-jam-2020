using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance;
    
    public GameObject player;
    public GameObject ground;
    public GameObject gunPlane;
    public GameObject camera;
    public GameObject healthBar;
    public GameObject ammoCount;
    public Transform bulletParent;
    
    public GameObject magmaHazard;
    public GameObject barrelHazard;
    public Transform hazardParent;
    
    public float maxHealth = 100.0f;
    public float currentHealth = 50.0f;
    public int maxShotgunAmmo = 20;
    public int currentShotgunAmmo = 10;
    public float magmaExpiryDecrement = 0.1f;
    public float magmaExpiryTime = 3.0f;
    public int maxSpawnAttempts = 10;
    public float hazardSpawnDistanceMin = 5.0f;
    public float hazardSpawnDistanceMax = 15.0f;
    public float barrelGroupDistance = 10.0f;
    public int numBarrelsInGroup = 3;
    
    public Vector2 worldCenter = new Vector2(0, 0);
    [System.NonSerialized]
    public Transform playerTransform;
    [System.NonSerialized]
    public Rigidbody playerRb;
    [System.NonSerialized]
    public float worldWidth;
    [System.NonSerialized]
    public float worldLength;
    
    private float healthBarWidth;
    
    public Vector2 GetRandomWorldPoint() {
        float x = Random.Range(worldCenter.x - worldWidth / 2, worldCenter.x + worldWidth / 2);
        float y = Random.Range(worldCenter.y - worldLength / 2, worldCenter.y + worldLength / 2);
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
    
    private void SpawnRandomMagma() {
        Vector3 pos = GetPointAwayFromPlayer(hazardSpawnDistanceMin, 0.01f, hazardSpawnDistanceMax);
        GameObject magma = Instantiate(magmaHazard, hazardParent);
        magma.transform.position = pos;
        StartCoroutine(MagmaSpawn(magma));
        StartCoroutine(MagmaExpire(magma));
    }
    
    private void SpawnBarrelBunch() {
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
        UpdateUIHealth();
        UpdateUIShotgunAmmo();
    }
    
    void Start() {
        SpawnRandomMagma();
        SpawnRandomMagma();
        SpawnRandomMagma();
        SpawnRandomMagma();
        SpawnBarrelBunch();
        SpawnBarrelBunch();
        SpawnBarrelBunch();
        SpawnBarrelBunch();
    }
    
    public void DamagePlayer(float points) {
        currentHealth -= points;
        if(currentHealth < 0) {
            currentHealth = 0;
        }
        UpdateUIHealth();
    }
    
    public void OnShotgunFire() {
        currentShotgunAmmo--;
        UpdateUIShotgunAmmo();
    }
    
    public void OnGameEnd() {
        //player.GetComponent<SteeringManager>().maxVelocity = 0;
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
        Destroy(magma);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}