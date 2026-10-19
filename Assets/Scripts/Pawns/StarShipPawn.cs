using UnityEngine;

public class StarShipPawn : Pawn
{
    
    // Min and max teleport values
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;
    
    // Movement speeds
    public float moveForce = 5f;
    public float rotateSpeed = 360f;
    
    // Shooting
    private Shooter shooter;
    
    // rigid body
    private Rigidbody rb;
    
    public void Start()
    {
        shooter = GetComponent<Shooter>();
        rb = GetComponent<Rigidbody>();
        minX = GameManager.instance.worldBoundaryMinX;
        maxX = GameManager.instance.worldBoundaryMaxX;
        minY = GameManager.instance.worldBoundaryMinY;
        maxY = GameManager.instance.worldBoundaryMaxY;
        GameManager.instance.playerGameObject = gameObject;
    }

    public void Update()
    {
        if (!GameManager.instance.playerGameObject)
        {
            GameManager.instance.playerGameObject = gameObject;
        }
    }

    public override void Move(Vector3 moveVector)
    {
        Vector3 pushForce = moveVector * moveForce;
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

    public override void Shoot()
    {
        if (shooter)
        {
            shooter.Shoot();
        }
    }
}
