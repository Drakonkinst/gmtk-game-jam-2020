using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steerable : MonoBehaviour
{
    public string modelName = "ChickenModel";
    [System.NonSerialized]
    public SteeringManager steering;
    [System.NonSerialized]
    public Transform myTransform;
    
    private Transform model;
    private bool isAvoiding = false;
    private Vector3 lastGoodPosition;

    void Start()
    {
        myTransform = transform;
        steering = GetComponent<SteeringManager>();
        if(steering == null) {
            Debug.LogError("No steering manager found!");
        }
        model = myTransform.Find(modelName);
        if(model == null) {
            Debug.LogWarning("No steerable model found!");
        }
    }

    void Update()
    {
        if(!SteeringManager.IsValid(SteeringManager.ToVector2(myTransform.position)) && !steering.ignoreWalls) {
            myTransform.position = lastGoodPosition;
        } else {
            lastGoodPosition = myTransform.position;
        }
        DoBoundaryBehavior();
        if(!isAvoiding) {
            DoBehavior();
        }
        
        if(model != null) {
            model.rotation = Quaternion.Euler(0, -steering.facing * Mathf.Rad2Deg, 0);
        }
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
    
    public virtual void DoBehavior() {
        steering.Wander();
    }
}