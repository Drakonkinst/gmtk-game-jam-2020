using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Steerable
{
    private bool isFleeing = false;
    private Vector3 fleeTarget;
    
    public override void DoBehavior() {
        if(!isFleeing) {
            steering.Wander();
        } else {
            steering.Flee(fleeTarget);
        }
        
    }
    
    public void Flee(Vector3 target, float seconds) {
        fleeTarget = target;
        isFleeing = true;
        StartCoroutine(FleeTask(seconds));
    }
    
    private IEnumerator FleeTask(float seconds) {
        yield return new WaitForSeconds(seconds);
        steering.SetWanderAngleToFacing();
        isFleeing = false;
    }
}