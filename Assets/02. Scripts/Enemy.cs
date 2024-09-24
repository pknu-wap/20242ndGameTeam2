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
        // ���� ���� ���¶�� �۵� X
        if (!isLive)
            return;
        // ��ǥ�� ��ġ�� ���� ���� ��ġ�� ���� ���� ���� ����
        Vector2 dirVec = player.position - enemy.position;
        // dirVec�� normalized�ϴ� ������ ��� ������ ���̸� ����ȭ�س��ƾ� �̵� �ӵ��� �������� �����̴�.
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        // ���� nextVec��ŭ �̵�
        enemy.MovePosition(enemy.position + nextVec);
        enemy.velocity = Vector2.zero;
    }
}
