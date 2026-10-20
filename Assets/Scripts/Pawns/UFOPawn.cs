using UnityEngine;

public class UFOPawn : AIPawn
{
    
    private float moveSpeed = 1f;
    public float maxSpeed = 2f;
    public float minSpeed = 0.2f;
    public float turboSpeedMultiplier = 2f;
    public float turboDistance = 100f;
    public float rotateSpeed = 360f;
    
    public void Start()
    {
        moveSpeed = Random.Range(minSpeed, maxSpeed);
    }

    public override void Move(Vector3 targetPoint, bool useTurbo = false)
    {
        if (useTurbo)
        {
            transform.position += transform.forward * ((moveSpeed * turboSpeedMultiplier) * Time.deltaTime);
        }
        else
        {
            transform.position += targetPoint * (moveSpeed * Time.deltaTime);
        }
    }

    public override void Yaw(float amount)
    {
        throw new System.NotImplementedException();
    }

    public override void Pitch(float amount)
    {
        throw new System.NotImplementedException();
    }

    public override void Roll(float amount)
    {
        throw new System.NotImplementedException();
    }

    public override void Seek(Transform target)
    {
        RotateTowards(target.position);
        
        float distance = Vector3.Distance(target.position, transform.position);
        
        Move(transform.forward, distance >= turboDistance);
    }

    public override void RotateTowards(Vector3 target)
    {        
        Vector3 vectorToTarget = target - transform.position;

        Quaternion lookRotation = Quaternion.LookRotation(vectorToTarget, Vector3.up);
        
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotateSpeed * Time.deltaTime);
    }

    public override void Shoot()
    {
        throw new System.NotImplementedException();
    }
}

