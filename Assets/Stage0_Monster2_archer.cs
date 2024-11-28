using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage0_Monster2_archer : BaseEnemy
{
    public float speed; // 이동 속도
    public Rigidbody2D player;
    public float stopDistance; // 플레이어와의 거리
    public GameObject fireballPrefab; // fireballPrefab으로 이름 변경
    public Transform firePoint;
    public float fireRate = 2f; // 공격 속도
    public float nextFireTime;
    public float projectileSpeed = 10f; // 투사체 속도
    public int damageAmount = 1; // 공격력

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

        // 플레이어와의 방향 벡터 계산
        Vector2 dirVec = player.position - enemy.position;
        float distance = dirVec.magnitude;

        // 플레이어 위치에 따른 회전
        if (dirVec.x < 0)  // 플레이어가 왼쪽에 있을 때
        {
            enemy.transform.rotation = Quaternion.Euler(0, -180, 0);  // 적을 왼쪽으로 회전
        }
        else if (dirVec.x > 0)  // 플레이어가 오른쪽에 있을 때
        {
            enemy.transform.rotation = Quaternion.Euler(0, 0, 0);  // 적을 오른쪽으로 회전
        }

        // 플레이어가 멀리 있으면 이동
        if (distance > stopDistance && anim.GetBool("isWalk") && !anim.GetBool("isAttack"))
        {
            Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
            enemy.MovePosition(enemy.position + nextVec);
        }
        else
        {
            anim.SetBool("isWalk", false);
            if (Time.time >= nextFireTime && !isWaiting)
            {
                anim.SetBool("isAttack", true);
                StartCoroutine(WaitAndShoot());
            }
        }

        // 이동을 멈추고 정지 상태 유지
        enemy.velocity = Vector2.zero;
    }

    private IEnumerator WaitAndShoot()
    {
        isWaiting = true; // 대기 시작

        yield return new WaitForSeconds(1.3f);
        Shoot();
        nextFireTime = Time.time + 1f / fireRate;
        anim.SetBool("isAttack", false); // 공격 애니메이션 종료
        anim.SetBool("isWalk", true);
        isWaiting = false; // 대기 종료
    }

    void Shoot()
    {
        // fireballPrefab을 사용하여 투사체 객체 생성
        GameObject fireball = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);

        // 투사체의 스크립트 가져오기
        Projectile fireballScript = fireball.GetComponent<Projectile>();
        if (fireballScript != null)
        {
            // 플레이어와 발사 지점의 방향 계산
            Vector2 direction = new Vector2(player.position.x - firePoint.position.x, player.position.y - firePoint.position.y).normalized;

            // 투사체의 방향 설정
            fireballScript.direction = direction;
            fireballScript.speed = projectileSpeed;
            fireballScript.damageAmount = damageAmount;

            // 발사 각도 계산 (라디안으로 계산된 값을 각도로 변환)
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;  // y, x 방향에 대해 각도 계산
            fireball.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle)); // 각도에 맞게 회전
        }
    }

    protected override void Die()
    {
        base.Die();
        // 추가적인 사망 효과 구현
    }
}
