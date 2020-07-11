using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public SteeringManager steering;
    
    private Transform myTransform;
    private Transform model;
    private bool isAvoiding = false;
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Start");
        steering = GetComponent<SteeringManager>();
        if(steering == null) {
            Debug.LogError("No steering manager found!");
        }
        myTransform = transform;
        model = myTransform.Find("ChickenModel");
        if(model == null) {
            Debug.LogError("No player model found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        DoBoundaryBehavior();
        if(!isAvoiding) {
            DoWanderBehavior();
        }
        
        model.rotation = Quaternion.Euler(0, -steering.facing * Mathf.Rad2Deg, 0);
        steering.UpdateSteering();
    }
    
    private void DoBoundaryBehavior() {
        bool isOutOfBounds = !steering.CheckBounds();
        if(isOutOfBounds) {
            isAvoiding = true;
            steering.Seek(SceneManager.Instance.worldCenter, 0.0f);
        } else if(isAvoiding) {
            // not out of bounds
            isAvoiding = false;
            steering.SetWanderAngle(SceneManager.Instance.worldCenter);
        }
    }
    
    private void DoWanderBehavior() {
        steering.Wander();
    }
}
