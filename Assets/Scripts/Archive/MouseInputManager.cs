using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class MouseInputManager : MonoBehaviour
{
    /*
     * public static Vector2 mousePosition;
     * public static int width;
     * public static int height;
     * private static Transform playerTransform; 
     */
    //public static Quaternion playerRotation;
    private Transform myTransform;
    // Start is called before the first frame update
    void Start()
    {
        //width = Screen.width;
        //height = Screen.height;
        myTransform = transform;
        //playerTransform = GameObject.FindWithTag("Player").transform;
        //getInput();
    }
    /*
    // Update is called once per frame
    void Update()
    {
        getInput();
        //setPointerPosition();
    }

    void OnMouseDown()
    {
        //Debug.Log("MaxX: " + width);
        //Debug.Log("MaxY: " + height);
        Debug.Log("Position: " + mousePosition);

    }

    void getInput() // picked up by pointer object for player to use Transform.lookAt to get the proper rotation
    {
        Vector3 rawPosition = Input.mousePosition;
        mousePosition = new Vector2(Math.Abs(width / 2 - rawPosition.x), Math.Abs(height/2 - (rawPosition.y - height)));
    }

    void setPointerPosition()
    {
        float xComponent = (mousePosition.x / 2 + mousePositon.y / 2) - 5;
        float zComponent = (mousePosition.x / 2 + mousePositon.y / 2) - 5;

        pTransform.position = new Vector3(playerTransform.position.x,0, playerTransform.position.y);
    }*/

    void Update()
    {
        getRotation();
    }

    void getRotation()
    {
        //Get the Screen positions of the object
        Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);

        //Get the Screen position of the mouse
        Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);

        //Get the angle between the points
        float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);

        myTransform.rotation = Quaternion.Euler(new Vector3(0f, 225 - angle, 0f));
    }

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
}
