using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType1 : Steerable
{
    public float minTrackingDistance = 2.0f;
    public float slowingDistance = 1.0f;
    
    public override void DoBehavior() {
        float distance = Vector3.Distance(myTransform.position, SceneManager.Instance.playerTransform.position);
        if(distance <= minTrackingDistance) {
            FollowPlayer();
        } else {
            DoWanderBehavior();
        }
    }
    
    private void DoWanderBehavior() {
        steering.Wander();
    }
    
    private void FollowPlayer() {
        steering.Seek(SceneManager.Instance.playerTransform.position, slowingDistance);
    }
}