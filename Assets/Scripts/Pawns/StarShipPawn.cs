using UnityEngine;

public class StarShipPawn : Pawn
{
    // Movement speeds
    public float moveForce = 5f;
    public float turboMoveForce = 10f;
    public float rotateSpeed = 360f;
    
    // Shooting
    private Shooter shooter;
    
    // rigid body
    private Rigidbody rb;
    
    public void Start()
    {
        shooter = GetComponent<Shooter>();
        rb = GetComponent<Rigidbody>();
        GameManager.instance.playerGameObject = gameObject;
    }

    public void Update()
    {
        if (!GameManager.instance.playerGameObject)
        {
            GameManager.instance.playerGameObject = gameObject;
        }

        if (LevelData.instance)
        {
            if (transform.position.y > LevelData.instance.worldBoundaryY)
            {
                transform.position = new Vector3(transform.position.x, LevelData.instance.worldBoundaryY, transform.position.z);
            }
        }
    }

    public override void Move(Vector3 moveVector, bool useTurbo = false)
    {
        float forceToBeApplied = useTurbo ? turboMoveForce : moveForce;
        Vector3 pushForce = moveVector * forceToBeApplied;
        rb.AddForce(pushForce);
    }

    public override void Yaw(float amount)
    {
        transform.Rotate(0, amount * rotateSpeed * Time.deltaTime, 0);
    }

    public override void Pitch(float amount)
    {
        transform.Rotate(amount * rotateSpeed * Time.deltaTime, 0, 0);
    }

    public override void Roll(float amount)
    {
        transform.Rotate(0, 0, amount * rotateSpeed * Time.deltaTime);
    }

    public override void RotateTowards(Vector3 lookAtPoint)
    {
        throw new System.NotImplementedException();
    }

    public override void Shoot()
    {
        if (shooter)
        {
            shooter.Shoot();
        }
    }
}
