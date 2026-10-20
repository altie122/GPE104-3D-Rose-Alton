using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(100)]
public class ShipFollowCamera : MonoBehaviour
{
    [Header("Target")]
    public Transform ship;

    [Header("Camera Offset")]
    [Tooltip("Direction from the ship, in its local coordinate system.")]
    public Vector3 offsetDirection = new Vector3(0f, 0.25f, -1f);

    [Min(0.1f)]
    public float distance = 10f;

    [Min(0.1f)]
    public float minDistance = 3f;

    [Min(0.1f)]
    public float maxDistance = 40f;

    [Header("Zoom")]
    [Min(0f)]
    public float zoomSpeed = 10f;

    [Min(0f)]
    public float zoomSmoothTime = 0.08f;

    [Header("Following")]
    [Tooltip(
        "Rotation smoothing only. Position follows immediately. " +
        "Set to 0 to stay directly behind the ship."
    )]
    [Min(0f)]
    public float followSmoothTime = 0.08f;

    [Header("Look At")]
    [Tooltip(
        "Aim offset in the smoothed ship coordinate system. " +
        "Use zero to keep the ship centered."
    )]
    public Vector3 lookAtOffset = Vector3.zero;

    private InputSystem_Actions inputSystem;

    private Transform currentShip;
    private Quaternion followRotation;

    private float currentDistance;
    private float zoomVelocity;

    private bool initialized;

    private void Awake()
    {
        inputSystem = new InputSystem_Actions();
        ValidateSettings();
    }

    private void OnEnable()
    {
        inputSystem.Player.Enable();

        initialized = false;
        zoomVelocity = 0f;
    }

    private void OnDisable()
    {
        inputSystem.Player.Disable();
        initialized = false;
    }

    private void OnDestroy()
    {
        inputSystem?.Dispose();
    }

    private void Start()
    {
        SnapToTarget();
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

        distance = Mathf.Clamp(
            distance + zoomInput * zoomSpeed * Time.deltaTime,
            minDistance,
            maxDistance
        );
    }

    private void LateUpdate()
    {
        if (ship == null)
        {
            initialized = false;
            return;
        }

        if (!initialized || currentShip != ship)
        {
            SnapToTarget();
            return;
        }

        float deltaTime = Time.deltaTime;

        if (deltaTime <= 0f)
        {
            return;
        }

        // Smooth the reference rotation, not the world position.
        if (followSmoothTime <= 0f)
        {
            followRotation = ship.rotation;
        }
        else
        {
            float blend = 1f - Mathf.Exp(
                -deltaTime / followSmoothTime
            );

            followRotation = Quaternion.Slerp(
                followRotation,
                ship.rotation,
                blend
            );
        }

        if (zoomSmoothTime <= 0f)
        {
            currentDistance = distance;
            zoomVelocity = 0f;
        }
        else
        {
            currentDistance = Mathf.SmoothDamp(
                currentDistance,
                distance,
                ref zoomVelocity,
                zoomSmoothTime,
                Mathf.Infinity,
                deltaTime
            );
        }

        currentDistance = Mathf.Clamp(
            currentDistance,
            minDistance,
            maxDistance
        );

        ApplyCameraPose();
    }

    public void SnapToTarget()
    {
        if (ship == null)
        {
            initialized = false;
            return;
        }

        ValidateSettings();

        currentShip = ship;
        followRotation = ship.rotation;

        currentDistance = distance;
        zoomVelocity = 0f;

        initialized = true;

        ApplyCameraPose();
    }

    private void ApplyCameraPose()
    {
        Vector3 localDirection =
            offsetDirection.sqrMagnitude > 0.0001f
                ? offsetDirection.normalized
                : Vector3.back;

        // Anchor directly to the ship so movement cannot create lag.
        Vector3 cameraPosition =
            ship.position +
            followRotation * localDirection * currentDistance;

        // Use the same rotation frame for placement and aiming.
        Vector3 lookAtPoint =
            ship.position + followRotation * lookAtOffset;

        Vector3 lookDirection = lookAtPoint - cameraPosition;

        if (lookDirection.sqrMagnitude <= 0.0001f)
        {
            lookDirection = followRotation * Vector3.forward;
        }
        else
        {
            lookDirection.Normalize();
        }

        Vector3 cameraUp = followRotation * Vector3.up;

        // Avoid an unstable look rotation if forward and up align.
        if (
            Vector3.Cross(lookDirection, cameraUp).sqrMagnitude
            < 0.0001f
        )
        {
            cameraUp = followRotation * Vector3.forward;
        }

        Quaternion cameraRotation = Quaternion.LookRotation(
            lookDirection,
            cameraUp
        );

        transform.SetPositionAndRotation(
            cameraPosition,
            cameraRotation
        );
    }

    private void OnValidate()
    {
        ValidateSettings();
    }

    private void ValidateSettings()
    {
        minDistance = Mathf.Max(0.1f, minDistance);
        maxDistance = Mathf.Max(minDistance, maxDistance);

        distance = Mathf.Clamp(
            distance,
            minDistance,
            maxDistance
        );

        zoomSpeed = Mathf.Max(0f, zoomSpeed);
        zoomSmoothTime = Mathf.Max(0f, zoomSmoothTime);
        followSmoothTime = Mathf.Max(0f, followSmoothTime);
    }
}