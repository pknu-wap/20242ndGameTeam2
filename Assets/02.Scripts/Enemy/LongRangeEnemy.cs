using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongRangeEnemy : BaseEnemy
{
    public float speed; // 이동 속도
    public Rigidbody2D player;
    public float stopDistance; // 플레이어와의 최소 거리
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 2f; // 발사 간격
    public float nextFireTime;
    public float projectileSpeed = 10f; // 발사체 속도
    public int damageAmount = 1; // 플레이어에게 줄 기본적인 데미지

    bool isLive = true;
    Rigidbody2D enemy;

    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponent<Rigidbody2D>();
        damageMultiplier = 1.0f;
        expOnDeath = 50;
    }

    private void FixedUpdate()
    {
        if (!isLive)
            return;

        // 플레이어 방향 계산
        Vector2 dirVec = player.position - enemy.position;
        float distance = dirVec.magnitude;

        // 플레이어와 일정 거리 이상 떨어져 있으면 이동
        if (distance > stopDistance)
        {
            Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
            enemy.MovePosition(enemy.position + nextVec);
        }

        // 플레이어와 일정 거리 이내로 가까워지면 공격
        if (Time.time >= nextFireTime && distance <= stopDistance)
        {
            Shoot();
            nextFireTime = Time.time + 1f / fireRate;
        }

        // 적이 이동 중일 때 속도 0으로 설정하여 중력의 영향을 받지 않도록 한다
        enemy.velocity = Vector2.zero;
    }

    void Shoot()
    {
        // 발사체 생성
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        // 발사체 방향 설정
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            Vector2 direction = new Vector2(player.position.x - firePoint.position.x, player.position.y - firePoint.position.y).normalized;
            projectileScript.direction = direction;
            projectileScript.speed = projectileSpeed;
            projectileScript.damageAmount = damageAmount;
        }
    }

    // 적이 죽을 때 경험치를 추가하는 함수
    protected override void Die()
    {
        base.Die();  // BaseEnemy의 Die() 호출로 경험치를 추가
    }
}