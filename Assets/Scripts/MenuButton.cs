using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButton : MonoBehaviour
{
    private int cursorOn = 0;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(cursorOn == 0)
            {
                cursorOn = 2;
            }
            else
            {
                --cursorOn;
            }
        }
        if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (cursorOn == 2)
            {
                cursorOn = 0;
            }
            else
            {
                ++cursorOn;
            }
        }
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (cursorOn == 0)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
            }
            else if (cursorOn == 1)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("AboutScreen");
            }
            else if (cursorOn == 2)
            {
                Application.Quit();
            }
        }
    }
}
