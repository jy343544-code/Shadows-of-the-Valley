using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Combo")]
    [SerializeField] private float comboResetTime = 1.1f;
    [SerializeField] private float hitRadius = 1.15f;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayers;
    [SerializeField] private Animator animator;

    [Header("Combo damage")]
    [SerializeField] private int firstHitDamage = 18;
    [SerializeField] private int secondHitDamage = 22;
    [SerializeField] private int finisherDamage = 34;
    [SerializeField] private float knockback = 4f;
    [SerializeField] private float finisherLaunch = 5.5f;

    private int comboStep;
    private float lastAttackTime;

    // Hook this to the mobile Attack button.
    public void Attack()
    {
        if (Time.time - lastAttackTime > comboResetTime)
            comboStep = 0;

        comboStep = comboStep % 3 + 1;
        lastAttackTime = Time.time;

        if (animator != null)
            animator.SetInteger("ComboStep", comboStep);

        int damage = comboStep == 1 ? firstHitDamage :
                     comboStep == 2 ? secondHitDamage : finisherDamage;

        Vector3 center = attackPoint != null
            ? attackPoint.position
            : transform.position + transform.forward * 1.25f + Vector3.up;

        Collider[] hits = Physics.OverlapSphere(
            center, hitRadius, enemyLayers, QueryTriggerInteraction.Ignore);

        foreach (Collider hit in hits)
        {
            EnemyHealth enemy = hit.GetComponentInParent<EnemyHealth>();
            if (enemy == null) continue;

            Vector3 away = (enemy.transform.position - transform.position).normalized;
            Vector3 impact = away * knockback;
            float launch = comboStep == 3 ? finisherLaunch : 0f;
            enemy.TakeDamage(damage, impact, launch);
        }

        if (comboStep == 3)
            comboStep = 0;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 center = attackPoint != null
            ? attackPoint.position
            : transform.position + transform.forward * 1.25f + Vector3.up;
        Gizmos.DrawWireSphere(center, hitRadius);
    }
}
