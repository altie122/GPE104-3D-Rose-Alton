using System;
using UnityEngine;

public class AISeekerController : AIController
{
    private void Start()
    {
        controlledPawn = GetComponent<Pawn>();
        target = GameManager.instance.playerGameObject;
    }

    private void Update()
    {
        if (target)
        {
            Vector3 targetPosition = target.transform.position;
            Vector3 start = controlledPawn.transform.position;
            Vector3 vectorToTarget = targetPosition - start;
            
            controlledPawn.Move(vectorToTarget);
        }
        else
        {
            target = GameManager.instance.playerGameObject;
        }
    }
}
