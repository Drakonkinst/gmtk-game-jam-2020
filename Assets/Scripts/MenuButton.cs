using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
        }
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("AboutScreen");
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
