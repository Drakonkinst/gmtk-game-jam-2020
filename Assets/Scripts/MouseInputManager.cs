using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class MouseInputManager : MonoBehaviour
{
    public static Vector2 mousePosition;
    public static int width;
    public static int  height;
    // Start is called before the first frame update
    void Start()
    {
        width = Screen.width;
        height = Screen.height;
        getInput();
    }

    // Update is called once per frame
    void Update()
    {
        getInput();
    }

    void OnMouseDown()
    {
        //Debug.Log("MaxX: " + width);
        //Debug.Log("MaxY: " + height);
        //Debug.Log("Position: " + mousePosition);

    }

    void getInput()// picked up by player to use Transform.lookAt to get the proper rotation
    {
        Vector3 rawPosition = Input.mousePosition;
        mousePosition = new Vector2(rawPosition.x, Math.Abs(rawPosition.y - height));
    }
}
