using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseRangeEnemy : MonoBehaviour
{
    public float speed;
    public Rigidbody2D player;
    public float stopDistance;
    public int damageAmount = 1; // �÷��̾�� �� ������
    public float damageInterval = 1f; // �������� �ִ� ����
    private float nextDamageTime; // ���� �������� �ִ� �ð�

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

        // �Ÿ��� �ּ� �Ÿ����� ũ�� �̵�
        if (distance > stopDistance)
        {
            Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
            enemy.MovePosition(enemy.position + nextVec);
        }
        // �÷��̾�� ���� �� ������ �ֱ�
        else
        {
            if (Time.time >= nextDamageTime)
            {
                TakeDamage();
                nextDamageTime = Time.time + damageInterval; // ���� �������� �ִ� �ð� ����
            }
        }

        enemy.velocity = Vector2.zero;
    }
    // �÷��̾�� ������ �ֱ�
    private void TakeDamage()
    {
        PlayerManager playerManager = player.GetComponent<PlayerManager>();
        if (playerManager != null)
        {
            playerManager.TakeDamage(damageAmount, "���� ���� ����");
        }
    }
}
