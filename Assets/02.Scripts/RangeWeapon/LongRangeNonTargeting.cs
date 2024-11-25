using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongRangeNonTargeting : MonoBehaviour
{
    public float damage = LongRangeAttack3.arrowDamage;  // 공격력
    public int pierceCount = 0;  // 관통 수

    // 관통 수를 설정하는 메서드 추가
    public void SetPierceCount(int pierceAmount)
    {
        pierceCount = pierceAmount;  // 관통 수 설정
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        BaseEnemy enemy = other.GetComponent<BaseEnemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);  // 적에게 피해를 줌
        }

        // 관통 로직: 관통 수가 0보다 크면 충돌 후 계속 날아가도록 처리
        if (pierceCount > 0)
        {
            pierceCount--;  // 관통 수를 감소시킴
            return;  // 더 이상 충돌하지 않음
        }

        // 관통이 끝나면 화살 파괴
        if (!other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
