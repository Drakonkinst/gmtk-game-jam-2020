using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeathScreen : MonoBehaviour
{
    public GameObject scoreText;
    
    private void UpdateScore(int value) {
        scoreText.GetComponent<TextMeshProUGUI>().SetText("Score: " + value);
    }
    // Start is called before the first frame update
    void Start()
    {
        int score = PlayerPrefs.GetInt("player_score");
        UpdateScore(score);
        
        //TODO play death sound
    }
}
