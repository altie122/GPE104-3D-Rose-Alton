using UnityEngine;

public abstract class Pawn : MonoBehaviour
{
    // Movement
    public abstract void Move(Vector3 moveVector);
    public abstract void Yaw(float amount);
    public abstract void Pitch(float amount);
    public abstract void Roll(float amount);
    
    // Shoot
    public abstract void Shoot();
}

