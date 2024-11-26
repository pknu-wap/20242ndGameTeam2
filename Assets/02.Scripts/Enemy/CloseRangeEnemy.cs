using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseRangeEnemy : BaseEnemy
{
    public float speed;
    public Rigidbody2D player;
    public float stopDistance;
    public int damageAmount = 1; // 플레이어에게 줄 기본적인 데미지
    public float damageInterval = 1f; // 데미지 간격
    private float nextDamageTime; // 다음 데미지 시간을 추적

    bool isLive = true;

    Rigidbody2D enemy;

    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponent<Rigidbody2D>();
        damageMultiplier = 1.0f;
        expOnDeath = 1000000;
    }

    private void FixedUpdate()
    {
        if (!isLive)
            return;

        Vector2 dirVec = player.position - enemy.position;
        float distance = dirVec.magnitude;

        // 플레이어와 일정 거리 이상 떨어져 있으면 이동
        if (distance > stopDistance)
        {
            Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
            enemy.MovePosition(enemy.position + nextVec);
        }
        // 플레이어와 가까워지면 데미지 주기
        else
        {
            if (Time.time >= nextDamageTime)
            {
                TakeDamageToPlayer(); // 플레이어에게 데미지 주기
                nextDamageTime = Time.time + damageInterval; // 다음 데미지 주는 시간 설정
            }
        }

        // 적이 이동 중일 때 속도 0으로 설정하여 중력의 영향을 받지 않도록 한다
        enemy.velocity = Vector2.zero;
    }

    // 플레이어에게 데미지 주는 함수
    private void TakeDamageToPlayer()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.TakeDamageToPlayer(damageAmount, "근거리 적 공격");
        }
    }

    // 적이 죽을 때 경험치 추가하는 로직
    protected override void Die()
    {
        base.Die();  // BaseEnemy의 Die()를 호출하여 경험치를 추가
    }
}
