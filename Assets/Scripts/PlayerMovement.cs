using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public SteeringManager steering;
    
    private bool isAvoiding = false;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
        steering = GetComponent<SteeringManager>();
        if(steering == null) {
            Debug.LogError("No steering manager found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        DoBoundaryBehavior();
        if(!isAvoiding) {
            Debug.Log("Wandering!");
            DoWanderBehavior();
        }
        steering.UpdateSteering();
    }
    
    private void DoBoundaryBehavior() {
        bool isOutOfBounds = !steering.CheckBounds();
        if(isOutOfBounds) {
            Debug.Log("Out of bounds!");
            isAvoiding = true;
            steering.Seek(steering.worldCenter, 0.0f);
        } else if(isAvoiding) {
            Debug.Log("No longer avoiding");
            // not out of bounds
            isAvoiding = false;
            steering.SetWanderAngle(steering.worldCenter);
        }
    }
    
    private void DoWanderBehavior() {
        steering.Wander();
    }
}
