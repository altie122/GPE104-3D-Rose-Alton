using UnityEngine;

public class DeathPawn : Death
{
    private Controller controller;
    
    public override void Start()
    {
        controller = GetComponent<Controller>();
    }

    public override void Update()
    {
        //
    }

    public override void Die()
    {
        if (controller != null)
        {
            controller.controlledPawn = null;
        }
        
        Destroy(gameObject);
    }
}