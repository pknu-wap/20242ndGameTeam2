using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongRangeNonTargeting : MonoBehaviour
{
    public float damage=LongRangeAttack3.arrowDamage;

    void OnTriggerEnter2D(Collider2D other)
    {
        BaseEnemy enemy = other.GetComponent<BaseEnemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        if (!other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
