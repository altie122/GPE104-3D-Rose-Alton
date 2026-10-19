using UnityEngine;

public class Damager : MonoBehaviour
{
    public float damageAmount;
    public bool isInstaKill;
    public string team;

    // Leave disabled for terrain. Enable for disposable obstacles.
    public bool destroyOnHit = false;

    private Obstacle obstacleToRemoveOnDeath;

    private void Start()
    {
        obstacleToRemoveOnDeath = GetComponent<Obstacle>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        TryDealDamage(collision.collider);
    }

    private void OnTriggerEnter(Collider other)
    {
        TryDealDamage(other);
    }

    private void TryDealDamage(Collider other)
    {
        Health otherHealthComponent = other.GetComponentInParent<Health>();
        Damager otherDamagerComponent =
            other.GetComponentInParent<Damager>();

        // Ignore objects without health.
        if (otherHealthComponent == null)
        {
            return;
        }

        // Ignore objects on the same team.
        if (
            otherDamagerComponent != null
            && otherDamagerComponent.team == team
        )
        {
            return;
        }

        float damage = isInstaKill
            ? otherHealthComponent.maxHealth
            : damageAmount;

        otherHealthComponent.TakeDamage(damage);

        if (!destroyOnHit)
        {
            return;
        }

        if (
            GameManager.instance != null
            && GameManager.instance.obstacleList != null
            && obstacleToRemoveOnDeath != null
        )
        {
            GameManager.instance.obstacleList.Remove(
                obstacleToRemoveOnDeath
            );
        }

        Destroy(gameObject);
    }
}