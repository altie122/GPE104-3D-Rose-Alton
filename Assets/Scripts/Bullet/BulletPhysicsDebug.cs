using System.Collections;
using UnityEngine;

public class BulletPhysicsDebug : MonoBehaviour
{
    private IEnumerator Start()
    {
        Rigidbody body = GetComponent<Rigidbody>();

        if (!body)
        {
            Debug.LogError(
                "Bullet has no Rigidbody on the same GameObject.",
                this
            );

            yield break;
        }

        int bodyCount = GetComponentsInChildren<Rigidbody>(true).Length;

        Debug.Log(
            $"Debugging {name}, instance {GetEntityId()}. " +
            $"Rigidbodies in this hierarchy: {bodyCount}",
            this
        );

        for (int sample = 0; sample < 5; sample++)
        {
            yield return new WaitForSeconds(0.1f);

            Debug.Log(
                $"Bullet sample {sample + 1}: " +
                $"position={body.position.ToString("F3")}, " +
                $"velocity={body.linearVelocity.ToString("F3")}, " +
                $"forward={transform.forward.ToString("F3")}, " +
                $"gravity={body.useGravity}, " +
                $"kinematic={body.isKinematic}, " +
                $"constraints={body.constraints}",
                this
            );
        }
    }
}