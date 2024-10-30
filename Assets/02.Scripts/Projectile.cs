using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed; // 투사체 속도
    public Vector2 direction;
    public float projectileLifeTime = 3f;
    public int damageAmount = 10;

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
            // 플레이어의 PlayerManager를 가져와서 데미지 주기
            PlayerManager playerManager = other.GetComponent<PlayerManager>();
            if (playerManager != null)
            {
                playerManager.TakeDamage(damageAmount, "투사체 공격");
            }

            Destroy(gameObject); // 충돌 후 투사체 파괴
        }
    }
}
