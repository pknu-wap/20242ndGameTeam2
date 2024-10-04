using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack_Melee : MonoBehaviour
{
    [SerializeField] private GameObject bullet; 
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private GameObject playerMidPos;

    [SerializeField] private float lastAttackTime;

    void Start()
    {
        
    }

    void Update()
    {
        DetectAndAttack();
    }

    void DetectAndAttack()  //재민님 playerattack과 동일 => 나중에 합쳐야함
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