using UnityEngine;

public class MoveInRandomDirectionOnSpawn : MonoBehaviour
{
    public Transform parentTransform;
    
    private Rigidbody2D rigidBody;
    
    public float movementSpeedMin;
    
    public float movementSpeedMax;
    
    private Quaternion simulatedRotation;
    
    private void Awake()
    {

        // If parent transform doesn't exist, return early
        if (!parentTransform)
        {
            return;
        }
        
        simulatedRotation = parentTransform.rotation * Quaternion.AngleAxis(Random.Range(0, 360), parentTransform.forward);
        
        rigidBody = GetComponent<Rigidbody2D>();

        if (rigidBody)
        {
            rigidBody.AddForce((simulatedRotation * Vector3.up) * Random.Range(movementSpeedMin, movementSpeedMax));
        }
    }
}
