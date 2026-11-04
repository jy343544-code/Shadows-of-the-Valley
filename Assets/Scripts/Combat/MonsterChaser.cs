using UnityEngine;

public class MonsterChaser : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float detectionRange = 9f;
    [SerializeField] private float attackRange = 1.7f;
    [SerializeField] private float moveSpeed = 2.2f;
    [SerializeField] private float turnSpeed = 8f;
    [SerializeField] private int damage = 12;
    [SerializeField] private float attackCooldown = 1.8f;
    private float nextAttackTime;

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
        if (target == null) return;
        Vector3 delta = target.position - transform.position;
        delta.y = 0f;
        float distance = delta.magnitude;
        if (distance > detectionRange) return;

        if (delta.sqrMagnitude > 0.01f)
        {
            Quaternion face = Quaternion.LookRotation(delta.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, face, turnSpeed * Time.deltaTime);
        }

        if (distance > attackRange)
            transform.position += delta.normalized * moveSpeed * Time.deltaTime;
        else if (Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + attackCooldown;
            PlayerHealth hp = target.GetComponent<PlayerHealth>();
            if (hp != null) hp.TakeDamage(damage);
        }
    }
}