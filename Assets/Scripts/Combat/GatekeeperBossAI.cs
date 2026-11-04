using UnityEngine;

// Gatekeeper boss: readable windups, melee strikes, shockwaves, and a faster second phase.
[RequireComponent(typeof(EnemyHealth))]
public class GatekeeperBossAI : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;
    [SerializeField] private float wakeRange = 14f;
    [SerializeField] private float preferredRange = 3.2f;
    [SerializeField] private float moveSpeed = 2.4f;
    [SerializeField] private float phaseTwoMoveSpeed = 3.4f;
    [SerializeField] private float turnSpeed = 7f;

    [Header("Attacks")]
    [SerializeField] private int meleeDamage = 18;
    [SerializeField] private float meleeRange = 2.6f;
    [SerializeField] private float meleeWindup = 0.75f;
    [SerializeField] private float meleeCooldown = 2.2f;
    [SerializeField] private int shockwaveDamage = 24;
    [SerializeField] private float shockwaveRadius = 5.5f;
    [SerializeField] private float shockwaveWindup = 1.35f;
    [SerializeField] private float shockwaveCooldown = 6f;
    [SerializeField] private LayerMask playerLayers;
    [SerializeField] private Animator animator;

    private EnemyHealth health;
    private float nextAttackTime;
    private float attackTimer;
    private float activeWindup;
    private bool preparingAttack;
    private bool preparingShockwave;
    private bool phaseTwo;
    private bool defeated;

    private void Awake()
    {
        health = GetComponent<EnemyHealth>();
    }

    private void Start()
    {
        if (target == null)
        {
            PlayerHealth player = FindFirstObjectByType<PlayerHealth>();
            if (player != null) target = player.transform;
        }
    }

    private void Update()
    {
        if (defeated || target == null || health == null) return;

        float healthRatio = (float)health.CurrentHealth / health.MaxHealth;
        if (!phaseTwo && healthRatio <= 0.5f)
        {
            phaseTwo = true;
            if (animator != null) animator.SetTrigger("PhaseTwo");
            Debug.Log("Gatekeeper enters Phase Two.");
        }

        Vector3 toTarget = target.position - transform.position;
        toTarget.y = 0f;
        float distance = toTarget.magnitude;
        if (distance > wakeRange) return;

        if (toTarget.sqrMagnitude > 0.01f)
        {
            Quaternion face = Quaternion.LookRotation(toTarget.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, face, turnSpeed * Time.deltaTime);
        }

        if (preparingAttack)
        {
            activeWindup -= Time.deltaTime;
            if (activeWindup <= 0f) ResolveAttack();
            return;
        }

        if (distance > preferredRange)
        {
            transform.position += toTarget.normalized *
                (phaseTwo ? phaseTwoMoveSpeed : moveSpeed) * Time.deltaTime;
        }

        if (Time.time < nextAttackTime) return;

        if (phaseTwo && distance > meleeRange * 1.4f)
            BeginShockwave();
        else if (distance <= meleeRange)
            BeginMelee();
        else if (phaseTwo)
            BeginShockwave();
    }

    private void BeginMelee()
    {
        preparingAttack = true;
        preparingShockwave = false;
        activeWindup = meleeWindup;
        nextAttackTime = Time.time + meleeCooldown;
        if (animator != null) animator.SetTrigger("HeavyAttack");
        Debug.Log("Gatekeeper telegraphs a heavy melee strike.");
    }

    private void BeginShockwave()
    {
        preparingAttack = true;
        preparingShockwave = true;
        activeWindup = shockwaveWindup;
        nextAttackTime = Time.time + shockwaveCooldown;
        if (animator != null) animator.SetTrigger("ShockwaveWindup");
        Debug.Log("Gatekeeper telegraphs a ground shockwave.");
    }

    private void ResolveAttack()
    {
        preparingAttack = false;
        if (preparingShockwave)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, shockwaveRadius,
                playerLayers, QueryTriggerInteraction.Ignore);
            foreach (Collider hit in hits)
            {
                PlayerHealth player = hit.GetComponentInParent<PlayerHealth>();
                if (player != null) player.TakeDamage(shockwaveDamage);
            }
            if (animator != null) animator.SetTrigger("ShockwaveRelease");
        }
        else
        {
            Vector3 center = transform.position + transform.forward * (meleeRange * 0.55f) + Vector3.up;
            Collider[] hits = Physics.OverlapSphere(center, meleeRange * 0.65f,
                playerLayers, QueryTriggerInteraction.Ignore);
            foreach (Collider hit in hits)
            {
                PlayerHealth player = hit.GetComponentInParent<PlayerHealth>();
                if (player != null) player.TakeDamage(meleeDamage);
            }
        }
    }

    public void MarkDefeated()
    {
        defeated = true;
        preparingAttack = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = preparingShockwave ? Color.red : new Color(1f, 0.55f, 0.1f);
        Gizmos.DrawWireSphere(transform.position, preparingShockwave ? shockwaveRadius : meleeRange);
    }
}