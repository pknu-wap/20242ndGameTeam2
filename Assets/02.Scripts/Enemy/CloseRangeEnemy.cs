using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseRangeEnemy : MonoBehaviour
{
    public float speed;
    public Rigidbody2D player;

    public float stopDistance;

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
        enemy.velocity = Vector2.zero;
    }
}
