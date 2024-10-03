using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject bullet; 
    public Transform firePoint;
    public float bulletSpeed = 10f;
    public float attackRange = 5f;
    public float attackCooldown = 1f;

    private float lastAttackTime;
    void Update()
    {
        DetectAndAttack();
    }

    void DetectAndAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, LayerMask.GetMask("Enemy"));
        if (hitEnemies.Length > 0 && Time.time >= lastAttackTime + attackCooldown)
        {
            Shoot(hitEnemies[0].transform); 
            lastAttackTime = Time.time;
        }
    }
    void Shoot(Transform target)
    {
        GameObject bullet_shoot = Instantiate(bullet, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet_shoot.GetComponent<Rigidbody2D>();

        Vector2 direction = (target.position - firePoint.position).normalized;
        rb.velocity = direction * bulletSpeed; 
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

}
