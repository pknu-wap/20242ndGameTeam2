using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed; // 투사체 속도
    public Vector2 direction;
    public float projectileLifeTime = 3f;

    void Start()
    {
        Destroy(gameObject, projectileLifeTime);
    }

    void Update()
    {
        // 방향에 속도와 Time.deltaTime을 곱해 투사체 이동
        transform.position += (Vector3)direction * speed * Time.deltaTime;

        // 투사체가 일정 거리 이상 멀어지면 파괴
        if (transform.position.magnitude > 50f)
        {
            Destroy(gameObject);
        }
    }

    // 트리거 충돌 감지
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
