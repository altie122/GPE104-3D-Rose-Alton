using UnityEngine;

public class UFOPawn : Pawn
{
    
    private float moveSpeed = 1f;
    public float maxSpeed = 2f;
    public float minSpeed = 0.2f;
    
    public void Start()
    {
        moveSpeed = Random.Range(minSpeed, maxSpeed);
    }

    public override void Move(Vector3 moveVector)
    {
        transform.position += moveVector * (moveSpeed * Time.deltaTime);
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

    public override void Shoot()
    {
        throw new System.NotImplementedException();
    }
}

