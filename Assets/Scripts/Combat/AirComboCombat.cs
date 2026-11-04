using UnityEngine;

// Mobile-friendly aerial launcher, air combo chain, and ground slam.
public class AirComboCombat : MonoBehaviour
{
    [Header("Targeting")]
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private float targetRange = 3.2f;
    [SerializeField] private float targetRadius = 1.6f;
    [SerializeField] private Transform attackOrigin;

    [Header("Aerial combat")]
    [SerializeField] private int launcherDamage = 16;
    [SerializeField] private int airHitDamage = 12;
    [SerializeField] private int slamDamage = 38;
    [SerializeField] private float launcherForce = 7f;
    [SerializeField] private float airHitForce = 2.5f;
    [SerializeField] private float slamRadius = 4f;
    [SerializeField] private float comboWindow = 1.15f;
    [SerializeField] private Animator animator;

    private int airComboStep;
    private float lastAirHit;
    private EnemyHealth currentTarget;

    // Connect to the mobile Launcher button: lifts a nearby enemy into the air.
    public void LaunchTarget()
    {
        currentTarget = FindTarget();
        if (currentTarget == null) return;
        Rigidbody body = currentTarget.GetComponent<Rigidbody>();
        if (body == null || body.isKinematic) return;

        currentTarget.TakeDamage(launcherDamage, transform.forward * 2f, launcherForce);
        airComboStep = 0;
        lastAirHit = Time.time;
        if (animator != null) animator.SetTrigger("AirLauncher");
    }

    // Tap repeatedly while airborne to chain attacks against the selected target.
    public void AirAttack()
    {
        if (currentTarget == null || !currentTarget.gameObject.activeInHierarchy)
            currentTarget = FindTarget();
        if (currentTarget == null) return;

        if (Time.time - lastAirHit > comboWindow) airComboStep = 0;
        airComboStep = airComboStep % 4 + 1;
        lastAirHit = Time.time;

        Vector3 direction = (currentTarget.transform.position - transform.position).normalized;
        float lift = airComboStep == 4 ? 3.5f : 0.8f;
        currentTarget.TakeDamage(airHitDamage + airComboStep * 2,
            direction * airHitForce, lift);

        if (animator != null) animator.SetInteger("AirComboStep", airComboStep);
        if (airComboStep == 4) airComboStep = 0;
    }

    // Finish an aerial sequence with a radial impact around Sekhem.
    public void GroundSlam()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, slamRadius,
            enemyLayers, QueryTriggerInteraction.Ignore);
        foreach (Collider hit in hits)
        {
            EnemyHealth enemy = hit.GetComponentInParent<EnemyHealth>();
            if (enemy == null) continue;
            Vector3 away = (enemy.transform.position - transform.position).normalized;
            enemy.TakeDamage(slamDamage, away * 5f, 2f);
        }
        airComboStep = 0;
        currentTarget = null;
        if (animator != null) animator.SetTrigger("GroundSlam");
    }

    private EnemyHealth FindTarget()
    {
        Vector3 origin = attackOrigin != null ? attackOrigin.position : transform.position + Vector3.up;
        Collider[] hits = Physics.OverlapSphere(origin + transform.forward * targetRange * 0.5f,
            targetRadius + targetRange * 0.5f, enemyLayers, QueryTriggerInteraction.Ignore);
        EnemyHealth bestTarget = null;
        float bestDistance = float.MaxValue;
        foreach (Collider hit in hits)
        {
            EnemyHealth enemy = hit.GetComponentInParent<EnemyHealth>();
            if (enemy == null) continue;
            Vector3 toEnemy = enemy.transform.position - transform.position;
            if (Vector3.Dot(transform.forward, toEnemy.normalized) < 0.1f) continue;
            float distance = toEnemy.sqrMagnitude;
            if (distance < bestDistance) { bestDistance = distance; bestTarget = enemy; }
        }
        return bestTarget;
    }
}