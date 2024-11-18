using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster2_AddAnimation : BaseEnemy
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
    bool isWaiting = false;

    Rigidbody2D enemy;
    Animator anim;

    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        damageMultiplier = 1.0f;
        anim.SetBool("isWalk", true);
        anim.SetBool("isAttack", false);
    }

    private void FixedUpdate()
    {
        if (!isLive)
            return;

        // ���� �÷��̾ ���� �̵�
        Vector2 dirVec = player.position - enemy.position;
        float distance = dirVec.magnitude;

        if (dirVec.x < 0)  // 플레이어가 왼쪽에 있을 때
        {
            enemy.transform.rotation = Quaternion.Euler(0, -180, 0);  // 적을 왼쪽으로 회전
        }
        else if (dirVec.x > 0)  // 플레이어가 오른쪽에 있을 때
        {
            enemy.transform.rotation = Quaternion.Euler(0, 0, 0);  // 적을 오른쪽으로 회전
        }  
        // �÷��̾�� ���� �Ÿ� �̻��� ���� �̵�
        if (distance > stopDistance && anim.GetBool("isWalk") && !anim.GetBool("isAttack"))
        {
            Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
            enemy.MovePosition(enemy.position + nextVec);
        }

        else
        {
            anim.SetBool("isWalk",false);
            if(Time.time >= nextFireTime && !isWaiting)
            {
                anim.SetBool("isAttack", true); 
                StartCoroutine(WaitAndShoot());
            }
        }

        // ���� �ӵ��� 0���� ����
        enemy.velocity = Vector2.zero;
    }

    private IEnumerator WaitAndShoot()
    {
        isWaiting = true; // 대기 시작

        
        yield return new WaitForSeconds(0.65f);
        Shoot();
        nextFireTime = Time.time + 1f / fireRate;
        anim.SetBool("isAttack", false); // 공격 애니메이션 종료
        anim.SetBool("isWalk", true);
        isWaiting = false; // 대기 종료
    }

    void Shoot()
    {
        // 투사체 객체 생성
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        // 투사체의 스크립트 가져오기
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            // 플레이어와 발사 지점의 방향 계산
            Vector2 direction = new Vector2(player.position.x - firePoint.position.x, player.position.y - firePoint.position.y).normalized;

            // 투사체의 방향 설정
            projectileScript.direction = direction;
            projectileScript.speed = projectileSpeed;
            projectileScript.damageAmount = damageAmount;

            // 발사 각도 계산 (라디안으로 계산된 값을 각도로 변환)
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;  // y, x 방향에 대해 각도 계산
            projectile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle)); // 각도에 맞게 회전
        }
    }

    protected override void Die()
    {
        base.Die();
        // 추가적인 사망 효과 구현
    }
}
