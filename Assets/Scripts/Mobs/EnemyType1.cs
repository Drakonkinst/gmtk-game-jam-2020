using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class EnemyType1 : Steerable
{
    public float minTrackingDistance = 2.0f;
    public float slowingDistance = 1.0f;
    public float contactDistance = 1.0f;
    public float attackCooldown = 2.0f;
    public float damage = 5.0f;
    private Transform player;
    private Transform myTransform;
    private float nextAttack;
    
    public override void OnStart() {
        player = SceneManager.Instance.playerTransform;
        nextAttack = Time.time;
        myTransform = transform;
    }
    
    public override void DoBehavior() {
        float distance = Vector3.Distance(myTransform.position, SceneManager.Instance.playerTransform.position);
        Vector3 temp = myTransform.position;
        temp.y = 1.0f;
        myTransform.position = temp;
        if (distance <= minTrackingDistance) {
            FollowPlayer();
        } else {
            DoWanderBehavior();
        }
        
        if(Vector3.Distance(player.position, myTransform.position) <= contactDistance) {
            float currTime = Time.time;
            if(currTime > nextAttack) {
                SceneManager.Instance.DamagePlayer(damage);
                nextAttack = currTime + attackCooldown;
            }
        }
    }
    
    private void DoWanderBehavior() {
        steering.Wander();
    }
    
    private void FollowPlayer() {
        steering.Seek(SceneManager.Instance.playerTransform.position, slowingDistance);
    }
}