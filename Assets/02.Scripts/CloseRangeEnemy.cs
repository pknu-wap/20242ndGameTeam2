using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseRangeEnemy : MonoBehaviour
{
    public float speed;
    public Rigidbody2D player;
    public float stopDistance;
    public int damageAmount = 10; // 플레이어에게 줄 데미지
    public float damageInterval = 1f; // 데미지를 주는 간격
    private float nextDamageTime; // 다음 데미지를 주는 시간

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
        Vector2 dirVec = player.position - enemy.position;
        float distance = dirVec.magnitude;

        // 거리가 최소 거리보다 크면 이동
        if (distance > stopDistance)
        {
            Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
            enemy.MovePosition(enemy.position + nextVec);
        }
        // 플레이어와 접촉 시 데미지 주기
        else
        {
            if (Time.time >= nextDamageTime)
            {
                TakeDamage();
                nextDamageTime = Time.time + damageInterval; // 다음 데미지를 주는 시간 설정
            }
        }

        enemy.velocity = Vector2.zero;
    }
    // 플레이어에게 데미지 주기
    private void TakeDamage()
    {
        PlayerManager playerManager = player.GetComponent<PlayerManager>();
        if (playerManager != null)
        {
            playerManager.TakeDamage(damageAmount, "근접 적의 공격");
        }
    }
}
