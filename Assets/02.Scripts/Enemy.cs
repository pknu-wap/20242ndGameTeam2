using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed;
    public Rigidbody2D player;

    bool isLive = true;

    Rigidbody2D enemy;
    void Awake()
    {
        enemy = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // 적이 죽은 상태라면 작동 X
        if (!isLive)
            return;
        // 목표의 위치와 현재 적의 위치를 빼서 방향 벡터 설정
        Vector2 dirVec = player.position - enemy.position;
        // dirVec을 normalized하는 이유는 모든 방향의 길이를 정규화해놓아야 이동 속도가 같아지기 때문이다.
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        // 적을 nextVec만큼 이동
        enemy.MovePosition(enemy.position + nextVec);
        enemy.velocity = Vector2.zero;
    }
}
