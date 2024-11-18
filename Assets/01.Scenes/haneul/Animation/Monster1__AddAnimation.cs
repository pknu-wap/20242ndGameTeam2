using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster1__AddAnimation : BaseEnemy
{
    public float speed;
    public Rigidbody2D player;
    public float stopDistance;
    public int damageAmount = 1;
    public float damageInterval = 1f; // 공격 간격
    private float nextDamageTime;

    bool isLive = true;
    bool isWaiting = false; // 대기 중인지 확인하는 변수

    Animator anim; // 애니메이터 변수

    Rigidbody2D enemy;

    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); // 애니메이터 컴포넌트 가져오기
        damageMultiplier = 1.0f;
        anim.SetBool("isWalk", true);
        anim.SetBool("isAttack", false);
    }

    private void FixedUpdate()
    {
        if (!isLive)
            return;

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
        if (distance > stopDistance && anim.GetBool("isWalk") && !anim.GetBool("isAttack"))
        {
            // 플레이어에게 이동
            Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
            enemy.MovePosition(enemy.position + nextVec);
        }
        else
        {
            anim.SetBool("isWalk", false); // 걷기 애니메이션 종료
            if (Time.time >= nextDamageTime && !isWaiting) // 대기 중이지 않으면
            {
                anim.SetBool("isAttack", true); // 공격 애니메이션 시작
                StartCoroutine(WaitAndCheckCollision());
            }
        }

        enemy.velocity = Vector2.zero;
    }

    private IEnumerator WaitAndCheckCollision()
    {
        isWaiting = true; // 대기 시작

        // 0.5초 대기
        yield return new WaitForSeconds(0.8f);

        // 대기 후, 플레이어와의 충돌 여부 확인
        if (Enemy_MeleeAttack_Judgment.isAttackSusses)
        {
            TakeDamageToPlayer(); // 플레이어에게 피해 주기
            nextDamageTime = Time.time + damageInterval; // 다음 공격을 위한 시간 설정
        }
        yield return new WaitForSeconds(0.3f);
        anim.SetBool("isAttack", false); // 공격 애니메이션 종료
        anim.SetBool("isWalk", true);
        isWaiting = false; // 대기 종료
    }


    private void TakeDamageToPlayer()
    {
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.TakeDamageToPlayer(damageAmount, "근접 공격");
            Debug.Log("-HP");
        }
    }

    protected override void Die()
    {
        base.Die();
        // 추가적인 사망 효과 구현
    }
}
