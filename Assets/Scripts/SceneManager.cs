using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance;
    
    public GameObject player;
    public GameObject ground;
    public GameObject gunPlane;
    public GameObject camera;
    public GameObject healthBar;
    public Transform bulletParent;
    
    public float maxHealth = 100.0f;
    public float currentHealth = 50.0f;
    
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
        Debug.Log("HEALTHBAR: " + healthBarWidth);
        UpdateUIHealth();
    }
    
    public void DamagePlayer(float points) {
        UpdateUIHealth();
    }
    
    private void UpdateUIHealth() {
        healthBar.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, healthBarWidth * (currentHealth / maxHealth));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}