using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 targetOffset = new Vector3(0f, 1.45f, 0f);

    [Header("Orbit")]
    [SerializeField] private float distance = 4.2f;
    [SerializeField] private float sensitivity = 0.12f;
    [SerializeField] private float minPitch = -15f;
    [SerializeField] private float maxPitch = 65f;
    [SerializeField] private float smoothSpeed = 14f;

    private float yaw;
    private float pitch = 15f;
    private Vector2 pendingLook;

    public void AddLookInput(Vector2 delta)
    {
        pendingLook += delta;
    }

    private void Start()
    {
        if (target != null) yaw = target.eulerAngles.y;
    }

    private void LateUpdate()
    {
        if (target == null) return;

        yaw += pendingLook.x * sensitivity;
        pitch = Mathf.Clamp(pitch - pendingLook.y * sensitivity, minPitch, maxPitch);
        pendingLook = Vector2.zero;

        Quaternion orbit = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 focus = target.position + targetOffset;
        Vector3 desiredPosition = focus - orbit * Vector3.forward * distance;

        transform.position = Vector3.Lerp(
            transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(
            transform.rotation, orbit, smoothSpeed * Time.deltaTime);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
