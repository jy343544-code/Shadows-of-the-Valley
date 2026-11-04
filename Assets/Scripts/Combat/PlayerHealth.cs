using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float damageCooldown = 0.45f;
    public int CurrentHealth { get; private set; }
    public int MaxHealth => maxHealth;
    private float nextDamageTime;

    private void Awake() { CurrentHealth = maxHealth; }

    public void TakeDamage(int amount)
    {
        if (amount <= 0 || Time.time < nextDamageTime || CurrentHealth <= 0) return;
        nextDamageTime = Time.time + damageCooldown;
        CurrentHealth = Mathf.Max(0, CurrentHealth - amount);
        Debug.Log($"Sekhem HP: {CurrentHealth}/{maxHealth}");
        if (CurrentHealth == 0) Debug.Log("Sekhem defeated.");
    }

    public void RestoreHealth(int amount)
    {
        CurrentHealth = Mathf.Min(maxHealth, CurrentHealth + Mathf.Max(0, amount));
    }
}