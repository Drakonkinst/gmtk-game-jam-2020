using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringManager : MonoBehaviour {
    private static readonly float MaxAngle = 2.0f * Mathf.PI;
    
    public static Vector3 ToVector3(Vector2 vector) {
        return new Vector3(vector.x, 0.0f, vector.y);
    }
    
    public static Vector2 ToVector2(Vector3 vector) {
        return new Vector2(vector.x, vector.z);
    }
    
    public static Vector2 Truncate(Vector2 vector, float max) {
        if(vector.magnitude > max) {
            return vector.normalized * max;
        }
        return vector;
    }
    
    public static bool IsValid(Vector2 vector) {
        Vector2 center = SceneManager.Instance.worldCenter;
        float halfWidth = SceneManager.Instance.worldWidth / 2.0f;
        float halfLength = SceneManager.Instance.worldLength / 2.0f;
        float minX = center.x - halfWidth;
        float minY = center.x - halfLength;
        float maxX = center.x + halfWidth;
        float maxY = center.x + halfLength;
        
        //Debug.Log("[" + minX + "," + maxX + "]; [" + minY + "," + maxY + "]");
        
        return (vector.x >= minX && vector.x <= maxX
                && vector.y >= minY && vector.y <= maxY);
    }

    public GameObject host;
    public float facing = 0.0f;
    [System.NonSerialized]
    public float wanderAngle;
    public float maxForce = 0.1f;
    public float maxVelocity = 2.0f;
    public float wanderCircleDistance = 1.0f;
    public float wanderCircleRadius = 3.0f;
    public float maxAngleChangeDeg = 15.0f;
    public float sightDistance = 0.5f;
    public float sightAngleDeg = 30.0f;
    
    private float maxAngleChange;
    private float halfSight;
    private float sightAngle;
    
    private Transform hostTransform;
    private Rigidbody hostRb;
    private Vector2 steering;
    
    void Start() {
        this.host = gameObject;
        this.maxAngleChange = maxAngleChangeDeg * Mathf.Deg2Rad;
        this.halfSight = sightDistance / 2.0f;
        this.sightAngle = sightAngleDeg * Mathf.Deg2Rad;
        
        this.host = host;
        this.hostTransform = host.transform;
        this.hostRb = host.GetComponent<Rigidbody>();
        this.wanderAngle = Random.Range(0.0f, MaxAngle);
        this.facing = 0.0f;
        
        if(hostTransform == null || hostRb == null) {
            Debug.LogError("Steering manager host missing transform and/or rigidbody!");
        }
        
        Reset();
    }
    
    private void UpdateFacing() {
        Vector2 velocity = GetHostVelocity();
        if(velocity.magnitude > 0) {
            facing = Mathf.Atan2(velocity.y, velocity.x);
        }
    }
    
    private void Reset() {
        steering = new Vector2();
    }
    
    private Vector2 GetHostPos() {
        return ToVector2(hostTransform.position);
    }
    
    private Vector2 GetHostVelocity() {
        return ToVector2(hostRb.velocity);
    }
    
    public void UpdateSteering() {
        //Debug.Log("Updating!");
        steering = Truncate(steering, maxForce);
        
        hostRb.velocity += ToVector3(steering);
        hostRb.velocity = ToVector3(Truncate(GetHostVelocity(), maxVelocity));
        
        UpdateFacing();
        Reset();
    }
    
    public void Seek(Vector3 targetPos, float slowingRadius = 5.0f) {
        Seek(ToVector2(targetPos), slowingRadius);
    }
    
    public void Seek(Vector2 targetPos, float slowingRadius = 5.0f) {
        if(targetPos == null) {
            Debug.LogWarning("Null seek command!");
            return;
        }
        
        Vector2 hostPos = GetHostPos();
        //float maxVelocity = host.GetMaxVelocity();
        float distance = Vector2.Distance(hostPos, targetPos);
        Vector2 seekForce = (targetPos - hostPos).normalized * maxVelocity;
        
        if(distance < slowingRadius) {
            seekForce *= (distance / slowingRadius);
        }
        
        steering += seekForce - GetHostVelocity();
    }
    
    public void Flee(Vector3 targetPos) {
        Flee(ToVector2(targetPos));
    }
    
    public void Flee(Vector2 targetPos) {
        Vector2 fleeForce = (GetHostPos() - targetPos).normalized * maxVelocity;
        steering += fleeForce - GetHostVelocity();
    }
    
    public void SetWanderAngle(Vector3 towards) {
        SetWanderAngle(ToVector2(towards));
    }
    
    public void SetWanderAngle(Vector2 towards) {
        Vector2 hostPos = GetHostPos();
        wanderAngle = Mathf.Atan2(towards.y - hostPos.y, towards.x - hostPos.x);
    }
    
    public void Wander() {
        Vector2 circleCenter = GetHostVelocity().normalized * wanderCircleDistance;
        
        wanderAngle += Random.Range(-maxAngleChange, maxAngleChange);
        wanderAngle = wanderAngle % MaxAngle;
        if(wanderAngle < 0) {
            wanderAngle += MaxAngle;
        }
        
        Vector2 displacement = new Vector2(Mathf.Cos(wanderAngle), Mathf.Sin(wanderAngle)) * wanderCircleRadius;
        steering += circleCenter + displacement;
    }
    
    // returns true if everything is good
    public bool CheckBounds() {
        Vector2 hostPos = GetHostPos();
        Vector2 ahead = GetHostVelocity() * sightDistance;
        Vector2 ahead2 = ahead / 2.0f;
        
        // these lefts and rights could be mixed up
        float leftAngle = facing - sightAngle;
        float rightAngle = facing + sightAngle;
        Vector2 aheadLeft = new Vector2(Mathf.Cos(leftAngle), Mathf.Sin(leftAngle)) * halfSight;
        Vector2 aheadRight = new Vector2(Mathf.Cos(rightAngle), Mathf.Sin(rightAngle)) * halfSight;

        ahead += hostPos;
        ahead2 += hostPos;
        aheadLeft += hostPos;
        aheadRight += hostPos;
        
        if(!IsValid(ahead)) {
            return false;
        }
        if(!IsValid(ahead2)) {
            return false;
        }
        if(!IsValid(aheadLeft)) {
            return false;
        }
        if(!IsValid(aheadRight)) {
            return false;
        }
        
        return true;
    }
}