using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon_0 : MonoBehaviour
{
    [Header("Weapon Settings")]
    public float attackRange_x = 1.5f;      // 공격 범위 (가로 크기)
    [SerializeField] private float attackRange_y = 1.5f;     // 공격 범위 (세로 크기)

    public int damage = 10;              // 공격력
    public float attackCooldown = 0.5f;  // 공격 딜레이

    [Header("References")]
    public Transform meleePivot;         // 공격의 중심이 될 지점

    [Header("Layer Settings")]
    public LayerMask enemyLayer;         // 공격 대상 레이어

    private float lastAttackTime;        // 마지막 공격 시간 기록

    // Update is called once per frame
    void Update()
    {
        // 마우스 클릭 또는 키 입력으로 공격 트리거
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            Attack();
        }
    }

    void Attack()
    {
        // 공격 애니메이션 실행

        // 정사각형 범위 내의 적을 탐지
        Vector2 boxSize = new Vector2(attackRange_x, attackRange_y);
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(meleePivot.position, boxSize, 0, enemyLayer);

        // 탐지된 적에게 피해를 줌
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log($"맞은 적: {enemy.name}, 데미지: {damage}");
            // 적의 데미지 처리 로직 호출
            enemy.GetComponent<BaseEnemy>().TakeDamage(damage);
        }
    }

    // 공격 범위 시각화 (디버깅용)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 boxSize = new Vector3(attackRange_x, attackRange_y, 0);
        Gizmos.DrawWireCube(meleePivot.position, boxSize);
    }
}
