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
    private Scanner scanner;

    private void Start()
    {
        scanner = GetComponent<Scanner>();
    }
    void Update()
    {
        Transform target = scanner.nearestTarget;

        if (target != null && Time.time >= lastAttackTime + attackCooldown && GameManager.isMelee == false)
        {
            Shoot(target);
            lastAttackTime = Time.time;
        }
    }

   
    void Shoot(Transform target)
    {
        Vector2 targetPosition = target.position;

        GameObject bullet_shoot = Instantiate(bullet, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet_shoot.GetComponent<Rigidbody2D>();

        Vector2 direction = (targetPosition - (Vector2)firePoint.position).normalized;
        rb.velocity = direction * bulletSpeed; 
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
