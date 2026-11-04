using UnityEngine;

// Optional soft target lock for touch controls.
public class TargetLock : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private float lockRange = 16f;
    [SerializeField] private float maxLockAngle = 65f;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform lockMarker;
    [SerializeField] private float turnSpeed = 10f;

    public Transform CurrentTarget { get; private set; }

    private void Awake()
    {
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;
        if (lockMarker != null) lockMarker.gameObject.SetActive(false);
    }

    // Connect to the Lock button. Press again to release.
    public void ToggleLock()
    {
        if (CurrentTarget != null) { ClearLock(); return; }
        AcquireTarget();
    }

    public void AcquireTarget()
    {
        Vector3 origin = cameraTransform != null ? cameraTransform.position : transform.position + Vector3.up;
        Vector3 view = cameraTransform != null ? cameraTransform.forward : transform.forward;
        Collider[] hits = Physics.OverlapSphere(transform.position, lockRange, enemyLayers,
            QueryTriggerInteraction.Ignore);

        Transform best = null;
        float bestScore = float.MaxValue;
        foreach (Collider hit in hits)
        {
            EnemyHealth enemy = hit.GetComponentInParent<EnemyHealth>();
            if (enemy == null) continue;
            Vector3 direction = enemy.transform.position - origin;
            float angle = Vector3.Angle(view, direction);
            if (angle > maxLockAngle) continue;
            float score = angle * 2f + direction.magnitude;
            if (score < bestScore) { bestScore = score; best = enemy.transform; }
        }

        CurrentTarget = best;
        if (lockMarker != null) lockMarker.gameObject.SetActive(CurrentTarget != null);
    }

    public void ClearLock()
    {
        CurrentTarget = null;
        if (lockMarker != null) lockMarker.gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        if (CurrentTarget == null) return;
        if (!CurrentTarget.gameObject.activeInHierarchy)
        {
            ClearLock();
            return;
        }

        if (lockMarker != null)
            lockMarker.position = CurrentTarget.position + Vector3.up * 2f;

        Vector3 direction = CurrentTarget.position - transform.position;
        direction.y = 0f;
        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion desired = Quaternion.LookRotation(direction.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, desired, turnSpeed * Time.deltaTime);
        }
    }
}