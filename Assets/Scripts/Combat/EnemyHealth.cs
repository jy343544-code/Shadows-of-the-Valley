using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 80;
    [SerializeField] private bool destroyOnDefeat = true;
    [SerializeField] private Rigidbody body;

    private int currentHealth;
    private bool defeated;

    private void Awake()
    {
        currentHealth = Mathf.Max(1, maxHealth);
        if (body == null) body = GetComponent<Rigidbody>();
    }

    public void TakeDamage(int amount)
    {
        TakeDamage(amount, Vector3.zero, 0f);
    }

    public void TakeDamage(int amount, Vector3 impact, float upwardLaunch)
    {
        if (amount <= 0 || defeated) return;

        currentHealth = Mathf.Max(0, currentHealth - amount);
        Debug.Log($"{name}: {currentHealth}/{maxHealth} HP");

        if (body != null && body.isKinematic == false)
            body.AddForce(impact + Vector3.up * upwardLaunch, ForceMode.Impulse);

        if (currentHealth == 0)
            Defeat();
    }

    private void Defeat()
    {
        defeated = true;
        if (destroyOnDefeat) Destroy(gameObject, 0.15f);
        else gameObject.SetActive(false);
    }
}
