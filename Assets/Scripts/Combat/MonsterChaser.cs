using UnityEngine;

public class MonsterChaser : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Rigidbody body;
    [SerializeField] private float detectionRange = 9f;
    [SerializeField] private float attackRange = 1.7f;
    [SerializeField] private float moveSpeed = 2.2f;
    [SerializeField] private float turnSpeed = 8f;
    [SerializeField] private int damage = 12;
    [SerializeField] private float attackCooldown = 1.8f;
    private float nextAttackTime;

    private void Awake()
    {
        if (body == null) body = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        if (target == null)
        {
            PlayerHealth player = FindFirstObjectByType<PlayerHealth>();
            if (player != null) target = player.transform;
        }
    }

    private void FixedUpdate()
    {
        if (target == null) return;

        Vector3 delta = target.position - transform.position;
        delta.y = 0f;
        float distance = delta.magnitude;
        if (distance > detectionRange) return;

        if (delta.sqrMagnitude > 0.01f)
        {
            Quaternion face = Quaternion.LookRotation(delta.normalized);
            Quaternion rotated = Quaternion.Slerp(
                body != null ? body.rotation : transform.rotation,
                face, turnSpeed * Time.fixedDeltaTime);

            if (body != null) body.MoveRotation(rotated);
            else transform.rotation = rotated;
        }

        if (distance > attackRange)
        {
            Vector3 next = transform.position + delta.normalized * moveSpeed * Time.fixedDeltaTime;
            if (body != null) body.MovePosition(next);
            else transform.position = next;
        }
        else if (Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown;
            PlayerHealth hp = target.GetComponent<PlayerHealth>();
            if (hp != null) hp.TakeDamage(damage);
        }
    }
}