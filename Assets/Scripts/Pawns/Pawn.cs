using UnityEngine;

public abstract class Pawn : MonoBehaviour
{
    // Movement
    public abstract void Move(Vector3 moveVector, bool useTurbo = false);
    public abstract void Yaw(float amount);
    public abstract void Pitch(float amount);
    public abstract void Roll(float amount);
    public abstract void RotateTowards(Vector3 lookAtPoint);
    
    // Shoot
    public abstract void Shoot();
}

