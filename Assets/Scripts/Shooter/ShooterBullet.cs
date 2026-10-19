using UnityEngine;

public class ShooterBullet : Shooter
{
    public Transform bulletSpawnPoint;
    
    public GameObject bulletPrefab;
    
    public override void Start()
    {
    }

    public override void Update()
    {
    }

    public override void Shoot()
    {
        if (bulletPrefab && bulletSpawnPoint)
        {
            Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        }
    }
}
