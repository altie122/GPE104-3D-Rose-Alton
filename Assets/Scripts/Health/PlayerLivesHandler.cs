using UnityEngine;

public class PlayerLivesHandler : MonoBehaviour
{
    public Health healthComponent;
    public Death deathComponent;
    
    void Update()
    {
        if (LevelData.instance && healthComponent && deathComponent)
        {
            if (healthComponent.currentHealth <= 0)
            {
                // Debug.Log("Player Lives Handler: - 1 life");
                healthComponent.currentHealth = healthComponent.maxHealth;
                transform.position = new Vector3(0, 75, 0);
                LevelData.instance.playerLives--;
            }

            if (LevelData.instance.playerLives <= 0)
            {
                // Debug.Log("Player Lives Handler: Death");
                LevelData.instance.isPlayerAlive = false;
                deathComponent.Die();
            }
        }
    }
}
