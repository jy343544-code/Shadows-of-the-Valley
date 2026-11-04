using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 80;
    [SerializeField] private bool destroyOnDefeat = true;
    [SerializeField] private Rigidbody body;

    public int CurrentHealth { get; private set; }
    public int MaxHealth => maxHealth;
    public bool IsDefeated { get; private set; }

    private void Awake()
    {
        CurrentHealth = Mathf.Max(1, maxHealth);
        if (body == null) body = GetComponent<Rigidbody>();
    }

    public void TakeDamage(int amount) => TakeDamage(amount, Vector3.zero, 0f);

    public void TakeDamage(int amount, Vector3 impact, float upwardLaunch)
    {
        if (amount <= 0 || IsDefeated) return;

        CurrentHealth = Mathf.Max(0, CurrentHealth - amount);
        if (body != null && !body.isKinematic)
            body.AddForce(impact + Vector3.up * upwardLaunch, ForceMode.Impulse);

        if (CurrentHealth == 0) Defeat();
    }

    private void Defeat()
    {
        IsDefeated = true;
        if (destroyOnDefeat) Destroy(gameObject, 0.15f);
        else gameObject.SetActive(false);
    }
}