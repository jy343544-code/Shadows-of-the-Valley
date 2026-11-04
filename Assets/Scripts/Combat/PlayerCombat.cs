using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private float cooldown = 0.55f;
    [SerializeField] private float attackRadius = 0.85f;
    [SerializeField] private int damage = 25;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayers;

    [Header("Animation (optional)")]
    [SerializeField] private Animator animator;
    [SerializeField] private string attackTrigger = "Attack";

    private float nextAttackTime;

    // Connect this to the on-screen Attack button.
    public void Attack()
    {
        if (Time.time < nextAttackTime) return;
        nextAttackTime = Time.time + cooldown;

        if (animator != null && !string.IsNullOrEmpty(attackTrigger))
            animator.SetTrigger(attackTrigger);

        Vector3 center = attackPoint != null
            ? attackPoint.position
            : transform.position + transform.forward * 1.25f + Vector3.up;

        Collider[] hits = Physics.OverlapSphere(
            center, attackRadius, enemyLayers, QueryTriggerInteraction.Ignore);

        foreach (Collider hit in hits)
        {
            EnemyHealth enemy = hit.GetComponentInParent<EnemyHealth>();
            if (enemy != null) enemy.TakeDamage(damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 center = attackPoint != null
            ? attackPoint.position
            : transform.position + transform.forward * 1.25f + Vector3.up;
        Gizmos.DrawWireSphere(center, attackRadius);
    }
}
