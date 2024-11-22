using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MeleeWeapon_0 : MonoBehaviour  // 채찍
{
    [Header("Weapon Settings")]
    private float attackRange_x = 1.5f;  // 공격 범위 (가로 크기)
    private float attackRange_y = 1.5f;  // 공격 범위 (세로 크기)
    [SerializeField] private int weaponLevel = 0;         // 무기 레벨

    [SerializeField] private int damage = 10;            // 기본 공격력
    [SerializeField] private float attackCooldown = 0.5f;   // 공격 딜레이
    [SerializeField] private float directionalAttackDelay = 0.2f; // 양방향 공격 간의 딜레이

    [Header("References")]
    [SerializeField] private Transform meleePivot;         // 공격의 중심이 될 지점
    [SerializeField] private LayerMask enemyLayer;         // 공격 대상 레이어
    [SerializeField] private Animator animator;           // Animator 연결

    private float lastAttackTime;        // 마지막 공격 시간 기록
    private bool isAttacking = false;

    void Start()
    {
        MeleeLevel(weaponLevel);  // 무기 레벨에 따른 설정 갱신
    }

    void Update()
    {
        if (!isAttacking && Time.time >= lastAttackTime + attackCooldown)
        {
            isAttacking = true;
            lastAttackTime = Time.time;
            StartCoroutine(AttackSequence());
        }
    }

    IEnumerator AttackSequence()
    {
        if (weaponLevel >= 2)
        {
            // 왼쪽 공격
            Attack(Vector2.left);
            yield return new WaitForSeconds(directionalAttackDelay);

            // 오른쪽 공격
            Attack(Vector2.right);
        }
        else
        {
            // 단일 방향 (기본 오른쪽)
            Attack(Vector2.left);
        }
    }

    // 방향 받아와서 공격하는 함수
    void Attack(Vector2 direction)
    {
        // 무기 레벨에 따른 설정 업데이트
        MeleeLevel(weaponLevel);

        // 애니메이션 트리거
        animator.SetInteger("WeaponLevel", weaponLevel);


        // 공격 방향 설정 (왼쪽 또는 오른쪽)
        //currentDirection = direction;

        // Transform 업데이트 (크기 및 위치 조정)
        UpdateAnimationTransform(direction);

        if(isAttacking)
        {
            // 애니메이션 실행
            if (direction == Vector2.left)
            {
                animator.SetBool("IsAttackingLeft", true);
                animator.SetBool("IsAttackingRight", false);
            }
            else if (direction == Vector2.right)
            {
                animator.SetBool("IsAttackingRight", true);
                animator.SetBool("IsAttackingLeft", false);
            }

            StartCoroutine(ResetAttackFlag());
        }

        // 방향에 따라 공격 범위 중심 계산
        Vector2 boxCenter = (Vector2)meleePivot.position + direction * (attackRange_x / 2);
        Vector2 boxSize = new Vector2(attackRange_x, attackRange_y);

        // 범위 내의 적 탐지
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0, enemyLayer);

        // 탐지된 적들에게 피해를 줌
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<BaseEnemy>().TakeDamage(damage);
        }
    }
    IEnumerator ResetAttackFlag()
    {
        //이부분 수정해서 에니메이션 종료 시간 계산까지 코루틴 기다리게 작업 필요.
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        /* while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null; // 애니메이션이 끝날 때까지 대기
        }*/
        animator.SetBool("IsAttackingLeft", false);
        animator.SetBool("IsAttackingRight", false);

        isAttacking = false;
    }

    void UpdateAnimationTransform(Vector2 currentDirection)
    {
        // 현재 공격 범위 크기
        Vector3 animationScale = new Vector3(attackRange_x, attackRange_y, 1);

        // 공격 범위 중심 좌표 (Pivot 위치 기준)
        Vector3 animationPosition = Vector3.zero;

        if (currentDirection == Vector2.left)
        {
            // 왼쪽 방향으로 이동
            animationPosition = new Vector3(-attackRange_x / 2, 0, 0); // 왼쪽으로 이동
        }
        else if (currentDirection == Vector2.right)
        {
            // 오른쪽 방향으로 이동
            animationPosition = new Vector3(attackRange_x / 2, 0, 0); // 오른쪽으로 이동
        }

        // Transform 업데이트
        meleePivot.localScale = animationScale;
        meleePivot.localPosition = animationPosition;

        Debug.Log($"Updated Animation: Scale={animationScale}, Position={animationPosition}");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 leftBoxCenter = (Vector2)meleePivot.position + Vector2.left * (attackRange_x / 2);
        Vector2 rightBoxCenter = (Vector2)meleePivot.position + Vector2.right * (attackRange_x / 2);
        Vector3 boxSize = new Vector3(attackRange_x, attackRange_y, 0);

        // 왼쪽 공격 범위
        Gizmos.DrawWireCube(leftBoxCenter, boxSize);

        // 오른쪽 공격 범위
        Gizmos.DrawWireCube(rightBoxCenter, boxSize);
    }

    private void MeleeLevel(int level)
    {
        // 무기 레벨에 따른 공격 범위 및 데미지 설정
        switch (level)
        {
            case 1:
                attackRange_x = 5;
                attackRange_y = 2;
                break;
            case 3:
                damage = 15;
                break;
            case 4:
                damage = 20;
                attackRange_x = 5.5f;
                attackRange_y = 2.5f;
                break;
            case 5:
                damage = 25;
                break;
            case 6:
                attackRange_x = 6;
                attackRange_y = 3f;
                damage = 30;
                break;
            case 7:
                damage = 35;
                break;
            case 8:
                damage = 40;
                break;
            default:
                break;
        }
    }
}