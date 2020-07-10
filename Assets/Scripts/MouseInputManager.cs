using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Vector2 mousePosition;
    // Start is called before the first frame update
    void Start()
    {
        mousePosition = MouseEventBase<T0>.mousePosition;
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = MouseEventBase<T0>.mousePosition; // picked up by player to use Transform.lookAt to get the proper rotation
        Debug.Log("Rotation: " + mousePosition);
    }
}
