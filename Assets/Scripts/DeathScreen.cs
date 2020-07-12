using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeathScreen : MonoBehaviour
{
    public GameObject scoreText;
    public AudioClip deathSound;
    
    private void UpdateScore(int value) {
        scoreText.GetComponent<TextMeshProUGUI>().SetText("Score: " + value);
    }
    // Start is called before the first frame update
    void Start()
    {
        int score = PlayerPrefs.GetInt("player_score");
        UpdateScore(score);
        
        SoundManager.Instance.Play(deathSound, Camera.main.transform);
    }
    
    void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            PlayerPrefs.SetInt("player_score", 0);
            UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
        }
    }
}
