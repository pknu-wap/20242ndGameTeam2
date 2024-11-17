using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon_1 : MonoBehaviour
{
    [Header("Skill Settings")]
    public float range = 5f;               // 공격 범위
    public int damage = 50;                // 데미지
    public float delayBeforeAttack = 1f;   // 범위 표시 후 공격 대기 시간
    public float attackCooldown = 1.5f;  // 공격 딜레이
    private float lastAttackTime;       // 마지막 공격 시간 기록

    [Header("References")]
    public Transform attackOrigin;         // 공격의 시작 지점
    public LayerMask enemyLayer;           // 공격 대상 레이어
    public GameObject rangeIndicatorPrefab; // 범위 표시를 위한 프리팹 (선택)

    private GameObject rangeIndicator;     // 범위 표시 오브젝트

    void Update()
    {
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            StartCoroutine(ActivateDelayMeleeAttack());
        }
    }

    private IEnumerator ActivateDelayMeleeAttack()
    {
        // 1. 범위 표시 활성화
        if (rangeIndicatorPrefab != null)
        {
            rangeIndicator = Instantiate(rangeIndicatorPrefab, attackOrigin.position, Quaternion.identity);
            rangeIndicator.transform.localScale = new Vector3(range * 2, range * 2, 1); // 원 크기 설정
        }
        else
        {
            Debug.Log("범위 표시가 없습니다. Gizmos로 디버깅하세요.");
        }

        // 2. 일정 시간 대기
        yield return new WaitForSeconds(delayBeforeAttack);

        // 3. 범위 공격
        Attack();

        // 4. 범위 표시 제거
        if (rangeIndicator != null)
        {
            Destroy(rangeIndicator);
        }
    }

    private void Attack()
    {
        // 범위 내 적 탐지
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackOrigin.position, range, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log($"Hit {enemy.name} for {damage} damage!");
            enemy.GetComponent<BaseEnemy>().TakeDamage(damage);
        }
    }

    // 범위 표시 디버깅용 (Gizmos 사용)
    private void OnDrawGizmosSelected()
    {
        if (attackOrigin != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(attackOrigin.position, range);
        }
    }
}
