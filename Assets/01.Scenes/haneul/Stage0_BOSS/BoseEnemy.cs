using UnityEngine;

public class BoseEnemy : BaseEnemy
{
    public bool isInvincible = false;


    protected override void Awake()
    {
        base.Awake();
    }

    public override void TakeDamage(float damage)
    {
        if (!isInvincible)
        {
            base.TakeDamage(damage);
        }
    }

    protected override void Die()
    {
        base.Die();
    }
}