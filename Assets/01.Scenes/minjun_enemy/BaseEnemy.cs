using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    public float maxHealth = 100f;
   [SerializeField] protected float currentHealth;

    // 무기에 따라 다르게 적용할 수 있는 데미지 배율
    public float damageMultiplier = 1.0f;

    protected virtual void Awake()
    {
        Debug.Log("Awake 들어감");
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(float damage)
    {
        // 실제 입는 데미지에 배율을 적용
        float actualDamage = damage * damageMultiplier;
        currentHealth -= actualDamage;

        if (currentHealth <= 0)
        {
            Debug.Log("체력 0 이하");
            Die();
        }
    }

    protected virtual void Die()
    {
        // 적 사망 처리 로직
        Debug.Log($"{gameObject.name} is dead.");
        Destroy(gameObject);
    }
}