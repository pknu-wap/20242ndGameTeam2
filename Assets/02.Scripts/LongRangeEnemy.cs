using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongRangeEnemy : BaseEnemy
{
    public float speed; // �� �̵� �ӵ�
    public Rigidbody2D player;
    public float stopDistance; // �÷��̾�� ���ߴ� �Ÿ�
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 2f; // �߻� ����
    public float nextFireTime;
    public float projectileSpeed = 10f; // ����ü �ӵ��� ������ ����
    public int damageAmount = 1; // �÷��̾�� �� ������

    bool isLive = true;
    Rigidbody2D enemy;

    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponent<Rigidbody2D>();
        damageMultiplier = 1.0f;
    }

    private void FixedUpdate()
    {
        if (!isLive)
            return;

        // ���� �÷��̾ ���� �̵�
        Vector2 dirVec = player.position - enemy.position;
        float distance = dirVec.magnitude;

        // �÷��̾�� ���� �Ÿ� �̻��� ���� �̵�
        if (distance > stopDistance)
        {
            Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
            enemy.MovePosition(enemy.position + nextVec);
        }

        // �߻� ���ݰ� �÷��̾���� �Ÿ��� ���� ����ü �߻�
        if (Time.time >= nextFireTime && distance <= stopDistance)
        {
            Shoot();
            nextFireTime = Time.time + 1f / fireRate;
        }

        // ���� �ӵ��� 0���� ����
        enemy.velocity = Vector2.zero;
    }

    void Shoot()
    {
        // ����ü ������ ����
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        // ����ü�� �ӵ��� ���� ����
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            Vector2 direction = new Vector2(player.position.x - firePoint.position.x, player.position.y - firePoint.position.y).normalized;
            projectileScript.direction = direction;
            projectileScript.speed = projectileSpeed;
            projectileScript.damageAmount = damageAmount;
        }
    }

    protected override void Die()
    {
        base.Die();
        // 추가적인 사망 효과 구현
    }
}
