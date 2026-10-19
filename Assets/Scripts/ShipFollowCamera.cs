using UnityEngine;
using UnityEngine.InputSystem;

public class ShipFollowCamera : MonoBehaviour
{
    [Header("Target")]
    public Transform ship;

    [Header("Camera Offset")]
    public Vector3 offsetDirection = new Vector3(0f, 1f, -1f);

    [Min(0.1f)] 
    public float distance = 20f;
    [Min(0.1f)] 
    public float minDistance = 5f;
    [Min(0.1f)] 
    public float maxDistance = 40f;

    [Header("Zoom")]
    [Min(0f)] 
    public float zoomSpeed = 10f;

    [Header("Following")]
    [Min(0.01f)]
    public float followSmoothTime = 0.2f;

    [Header("Look At")]
    public Vector3 lookAtOffset = new Vector3(0f, 0f, 10f);
    
    private InputSystem_Actions inputSystem;
    private Vector3 followVelocity;

    private void Awake()
    {
        inputSystem = new InputSystem_Actions();
        distance = Mathf.Clamp(distance, minDistance, maxDistance);
    }

    private void OnEnable()
    {
        inputSystem.Player.Enable();
        followVelocity = Vector3.zero;
    }

    private void OnDisable()
    {
        inputSystem.Player.Disable();
    }

    private void OnDestroy()
    {
        inputSystem.Dispose();
    }

    private void Start()
    {
        if (ship == null)
        {
            return;
        }

        transform.position = GetDesiredPosition();
        UpdateLookAt();
    }

    private void Update()
    {
        float zoomInput = 0f;

        if (inputSystem.Player.ZoomIn.IsPressed())
        {
            zoomInput -= 1f;
        }

        if (inputSystem.Player.ZoomOut.IsPressed())
        {
            zoomInput += 1f;
        }

        distance += zoomInput * zoomSpeed * Time.deltaTime;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);
    }

    private void LateUpdate()
    {
        if (!ship || Time.deltaTime <= 0f)
        {
            return;
        }

        transform.position = Vector3.SmoothDamp(
            transform.position,
            GetDesiredPosition(),
            ref followVelocity,
            followSmoothTime
        );

        UpdateLookAt();
    }

    private Vector3 GetDesiredPosition()
    {
        Vector3 localDirection =
            offsetDirection.sqrMagnitude > 0.0001f
                ? offsetDirection.normalized
                : Vector3.back;

        Vector3 worldDirection = ship.TransformDirection(localDirection);

        return ship.position + worldDirection * distance;
    }

    private void UpdateLookAt()
    {
        Vector3 lookAtPoint =
            ship.position + ship.rotation * lookAtOffset;

        Vector3 lookDirection = lookAtPoint - transform.position;

        if (lookDirection.sqrMagnitude > 0.0001f)
        {
            transform.rotation = Quaternion.LookRotation(
                lookDirection,
                Vector3.up
            );
        }
    }

    private void OnValidate()
    {
        minDistance = Mathf.Max(0.1f, minDistance);
        maxDistance = Mathf.Max(minDistance, maxDistance);
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        zoomSpeed = Mathf.Max(0f, zoomSpeed);
        followSmoothTime = Mathf.Max(0.01f, followSmoothTime);
    }
}