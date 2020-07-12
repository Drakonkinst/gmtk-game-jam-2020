using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemyType2 : Steerable
{
    public float minTrackingDistance = 10.0f;
    public float shootingDistance = 5.0f;
    public float slowingDistance = 1.0f;
    public float contactDistance = 1.0f;
    public float attackCooldown = 2.0f;
    public float shootingFrequency = 1.0f;
    private bool shooting = false;
    public float damage = 5.0f;
    private Transform player;
    private float nextAttack;
    private Transform childTransform;

    public override void OnStart()
    {
        player = SceneManager.Instance.playerTransform;
        childTransform = myTransform.Find("Model").Find("Handgun");
        if(childTransform == null)
        {
            Debug.Log("Child Not Found.");
        }
        nextAttack = Time.time;
    }

    public override void DoBehavior()
    {
        float distance = Vector3.Distance(myTransform.position, SceneManager.Instance.playerTransform.position);
        //childTransform.rotation = transform.rotation;
        if (distance <= minTrackingDistance && distance >= shootingDistance) // If the enemy is within the tracking and shooting distance
        {
            FollowPlayer(); // The enemy will follow
            if(!shooting)
            {
                StartCoroutine("ShootPlayer"); // And shoot at the player
            }
            shooting = true;
        }
        else if (distance < shootingDistance / 2)// If too close 
        {
            steering.Flee(SceneManager.Instance.playerTransform.position); // Flee while shooting
        }
        else // Otherwise
        {
            shooting = false;
            StopCoroutine("ShootPlayer"); // It will stop shooting
            DoWanderBehavior(); // And Wander until the enemy re-enters the previous range
        }
        
        if(Vector3.Distance(player.position, myTransform.position) <= contactDistance)
        {
            float currTime = Time.time;
            if (currTime > nextAttack)
            {
                Debug.Log("Attack!");
                SceneManager.Instance.DamagePlayer(damage);
                nextAttack = currTime + attackCooldown;
            }
        }
    }

    private void DoWanderBehavior()
    {
        steering.Wander();
    }

    private void FollowPlayer()
    {
        steering.Seek(SceneManager.Instance.playerTransform.position, slowingDistance);
    }

    IEnumerator ShootPlayer()
    {
        EnemyGun enemyGun = childTransform.gameObject.GetComponent<EnemyGun>();

        while (true)
        {
            myTransform.LookAt(player);
            enemyGun.Shoot();
            yield return new WaitForSeconds(shootingFrequency);

            myTransform.LookAt(player);
            enemyGun.Shoot();
            yield return new WaitForSeconds(shootingFrequency);

            myTransform.LookAt(player);
            enemyGun.Shoot();
            yield return new WaitForSeconds(shootingFrequency * 3);
        }
        // EnemyGun shoots in the direction the enemy is facing
    }
}