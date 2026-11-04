using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 60;
    [SerializeField] private bool destroyOnDefeat = true;

    private int currentHealth;

    private void Awake()
    {
        currentHealth = Mathf.Max(1, maxHealth);
    }

    public void TakeDamage(int amount)
    {
        if (amount <= 0 || currentHealth <= 0) return;

        currentHealth = Mathf.Max(0, currentHealth - amount);
        Debug.Log($"{name}: {currentHealth}/{maxHealth} HP");

        if (currentHealth == 0)
        {
            if (destroyOnDefeat) Destroy(gameObject);
            else gameObject.SetActive(false);
        }
    }
}
