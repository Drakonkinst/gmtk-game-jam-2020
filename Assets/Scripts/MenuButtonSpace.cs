using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonSpace : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Reached update");
        if(Input.GetKeyDown(KeyCode.Space)) {
            //Debug.Log("Transitioning to TitleScene");
            UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScene");
        }
    }
}
