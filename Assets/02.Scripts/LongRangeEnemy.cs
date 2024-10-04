using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongRangeEnemy : MonoBehaviour
{
    [SerializeField]
    private float speed; // �� �̵� �ӵ�
    [SerializeField]
    private Rigidbody2D player;
    public float stopDistance; // �÷��̾�� ���ߴ� �Ÿ�
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float fireRate = 2f; // �߻� ����
    public float nextFireTime;
    public float projectileSpeed = 10f; // ����ü �ӵ��� ������ ����

    bool isLive = true;
    private Rigidbody2D enemy;

    void Awake()
    {
        enemy = GetComponent<Rigidbody2D>();
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
        }
    }
}
