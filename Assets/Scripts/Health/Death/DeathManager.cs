using UnityEngine;

public class DeathManager : MonoBehaviour
{
    private Obstacle obstacleToRemoveOnDeath;

    public Death deathHandler;

    public int scoreValue;
    
    public void Start()
    {
        obstacleToRemoveOnDeath = GetComponent<Obstacle>();
        deathHandler = GetComponent<Death>();
    }
    
    public void Die()
    {
        if (GameManager.instance)
        {
            if (GameManager.instance.obstacleList != null && obstacleToRemoveOnDeath)
            {
                GameManager.instance.obstacleList.Remove(obstacleToRemoveOnDeath);
            }
            
            GameManager.instance.score += scoreValue;
        }

        if (deathHandler)
        {
            deathHandler.Die();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
