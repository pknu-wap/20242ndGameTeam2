using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongRangeEnemy : MonoBehaviour
{
    public float speed; // 적 이동 속도
    public Rigidbody2D player;
    public float stopDistance; // 플레이어와 멈추는 거리
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 2f; // 발사 간격
    public float nextFireTime;
    public float projectileSpeed = 10f; // 투사체 속도를 설정할 변수

    bool isLive = true;
    Rigidbody2D enemy;

    void Awake()
    {
        enemy = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!isLive)
            return;

        // 적이 플레이어를 향해 이동
        Vector2 dirVec = player.position - enemy.position;
        float distance = dirVec.magnitude;

        // 플레이어와 일정 거리 이상일 때만 이동
        if (distance > stopDistance)
        {
            Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
            enemy.MovePosition(enemy.position + nextVec);
        }

        // 발사 간격과 플레이어와의 거리에 따라 투사체 발사
        if (Time.time >= nextFireTime && distance <= stopDistance)
        {
            Shoot();
            nextFireTime = Time.time + 1f / fireRate;
        }

        // 적의 속도를 0으로 고정
        enemy.velocity = Vector2.zero;
    }

    void Shoot()
    {
        // 투사체 프리팹 생성
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        // 투사체의 속도와 방향 설정
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            Vector2 direction = new Vector2(player.position.x - firePoint.position.x, player.position.y - firePoint.position.y).normalized;
            projectileScript.direction = direction;
            projectileScript.speed = projectileSpeed;
        }
    }
}
