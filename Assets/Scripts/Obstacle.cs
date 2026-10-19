using UnityEngine;

public class Obstacle : MonoBehaviour
{
    
    private void Start()
    {
        GameManager.instance.obstacleList.Add(this);
    }
}
