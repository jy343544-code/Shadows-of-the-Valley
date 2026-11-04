using UnityEngine;

public class ShadowTether : MonoBehaviour
{
    [SerializeField] private float range = 7f;
    [SerializeField] private float cooldown = 4f;
    [SerializeField] private float pullImpulse = 7f;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private Transform castOrigin;
    private float nextUseTime;

    // Connect this to the on-screen Shadow Tether button.
    public void Cast()
    {
        if (Time.time < nextUseTime) return;
        nextUseTime = Time.time + cooldown;

        Vector3 origin = castOrigin != null ? castOrigin.position : transform.position + Vector3.up;
        Collider[] hits = Physics.OverlapSphere(
            origin + transform.forward * (range * 0.5f),
            range * 0.5f, enemyLayers, QueryTriggerInteraction.Ignore);

        EnemyHealth selected = null;
        float best = float.MaxValue;
        foreach (Collider hit in hits)
        {
            EnemyHealth enemy = hit.GetComponentInParent<EnemyHealth>();
            if (enemy == null) continue;
            Vector3 direction = enemy.transform.position - transform.position;
            if (Vector3.Dot(transform.forward, direction.normalized) < 0.25f) continue;
            if (direction.sqrMagnitude < best)
            {
                best = direction.sqrMagnitude;
                selected = enemy;
            }
        }

        if (selected == null) return;
        Rigidbody body = selected.GetComponent<Rigidbody>();
        if (body != null && !body.isKinematic)
        {
            Vector3 pull = (transform.position - selected.transform.position).normalized;
            body.AddForce(pull * pullImpulse + Vector3.up * 1.2f, ForceMode.Impulse);
        }
    }
}