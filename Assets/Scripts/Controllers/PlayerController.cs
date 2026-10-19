using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Controller
{
    // Input variables
    
    // Input system
    private InputSystem_Actions inputSystem;
    
    // Turn clockwise
    private InputAction rotateClockwiseAction;
    // Turn Counter-Clockwise
    private InputAction rotateCounterClockwiseAction;
    
    // Shoot
    private InputAction shootAction;
    
    public void MakeDecisions()
    {
        // Check if pawn exists, if not, return before control code runs
        if (!controlledPawn)
        {
            return;
        }
        
        if (inputSystem.Player.MoveForward.IsPressed())
        {
            controlledPawn.Move(controlledPawn.transform.forward);
        }
        if (inputSystem.Player.MoveBackward.IsPressed())
        {
            controlledPawn.Move(-controlledPawn.transform.forward);
        }

        if (rotateClockwiseAction.IsPressed())
        {
            controlledPawn.Yaw(1);
        }
        
        if (rotateCounterClockwiseAction.IsPressed())
        {
            controlledPawn.Yaw(-1);
        }

        if (inputSystem.Player.RollClockwise.IsPressed())
        {
            controlledPawn.Roll(-1);
        }

        if (inputSystem.Player.RollCounterClockwise.IsPressed())
        {
            controlledPawn.Roll(1);
        }

        if (inputSystem.Player.PitchUp.IsPressed())
        {
            controlledPawn.Pitch(-1);
        }

        if (inputSystem.Player.PitchDown.IsPressed())
        {
            controlledPawn.Pitch(1);
        }
            
        if (shootAction.WasPressedThisFrame())
        {
            controlledPawn.Shoot();
        }

        if (inputSystem.Player.Up.IsPressed())
        {
            controlledPawn.Move(controlledPawn.transform.up);
        }
    }
    
    public void Start()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.playerController = this;
        }
        inputSystem = new InputSystem_Actions();
        rotateClockwiseAction = inputSystem.Player.RotateClockwise;
        rotateCounterClockwiseAction = inputSystem.Player.RotateCounterClockwise;
        shootAction = inputSystem.Player.Shoot;
        inputSystem.Enable();
    }

    public void Update()
    {
        if (!GameManager.instance.isPaused)
        {
            MakeDecisions();
        }
    }

    public void OnDisable()
    {
        if (inputSystem != null)
        {
            inputSystem.Disable();
        }
    }
}
