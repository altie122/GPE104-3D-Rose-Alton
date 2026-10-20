using UnityEngine;

public class AISeekerController : AIController
{
    private AIPawn controlledPawn;
    private void Start()
    {
        controlledPawn = GetComponent<AIPawn>();
        target = GameManager.instance.playerGameObject;
    }

    private void Update()
    {
        if (target)
        {
            controlledPawn.Seek(target.transform);
        }
        else
        {
            target = GameManager.instance.playerGameObject;
        }
    }
}
