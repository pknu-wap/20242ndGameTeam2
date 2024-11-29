using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MeleeWeapon_1 : MonoBehaviour  // 채찍
{
    [Header("Weapon Settings")]
    [SerializeField] public int weaponLevel = 0;         // 무기 레벨
    [SerializeField] private int damage = 10;            // 기본 공격력
    [SerializeField] private float attackCooldown = 0.5f;   // 공격 딜레이
    [SerializeField] private float directionalAttackDelay = 0.2f; // 양방향 공격 간의 딜레이

    [Header("References")]
    [SerializeField] private Transform meleePivot;         // 공격의 중심이 될 지점
    [SerializeField] private Transform meleeTransform;      
    [SerializeField] private LayerMask enemyLayer;         // 공격 대상 레이어
    [SerializeField] private Animator animator;           // Animator 연결

    [Header("Fuxxing Debug")]
    public float attackRange_x = 1.5f;  // 공격 범위 (가로 크기)
    public float attackRange_y = 1.5f;  // 공격 범위 (세로 크기)


    [Header("Animator Override Controllers")]
    [SerializeField] private AnimatorOverrideController overrideController_Level4;
    [SerializeField] private AnimatorOverrideController overrideController_Level6;

    //디버깅용 임시 변수
    private Vector2 boxCenter;
    private Vector2 boxSize;
    private float lastAttackTime;        // 마지막 공격 시간 기록


    void Start()
    {
        weaponLevel = GameManager.Instance.meleeWeapon1_Level;
        MeleeLevel(weaponLevel);  // 무기 레벨에 따른 설정 갱신
    }

    void Update()
    {
        weaponLevel = GameManager.Instance.meleeWeapon1_Level;
        // 공격 쿨타임 체크 및 공격 트리거
        if (Time.time >= lastAttackTime + attackCooldown/*&& GameManager.isMelee*/)
        {
            lastAttackTime = Time.time;
            MeleeLevel(GameManager.Instance.meleeWeapon1_Level);
            animator.SetInteger("WeaponLevel", weaponLevel);

            StartCoroutine(AttackSequence());
        }
    }

    IEnumerator AttackSequence()
    {
        if (weaponLevel >= 2)
        {
            // 오른쪽 공격
            Attack(Vector2.right);
             yield return new WaitForSeconds(directionalAttackDelay);
            // 왼쪽 공격
            Attack(Vector2.left);
        }
        else if(weaponLevel == 1)
        {
            // 단일 방향 (기본 오른쪽)
            Attack(Vector2.right);
        }
    }

    // 방향 받아와서 공격하는 함수
    void Attack(Vector2 direction)
    {
        // 방향에 따라 공격 범위 중심 계산
        boxCenter = (Vector2)meleePivot.position + direction * (attackRange_x / 2);
        boxSize = new Vector2(attackRange_x, attackRange_y);

        // 애니메이션 실행
        animator.SetTrigger(direction == Vector2.left ? "Attack_L" : "Attack_R");

        // 범위 내의 적 탐지
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0, enemyLayer);

        // 탐지된 적들에게 피해를 줌
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<BaseEnemy>().TakeDamage(damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        // 왼쪽 공격 범위
        Gizmos.DrawWireCube(boxCenter, boxSize);
    }

    public void MeleeLevel(int level)
    {
        // 무기 레벨에 따른 공격 범위 및 데미지 설정
        switch (level)
        {
            case 0:
                attackRange_x = 0;
                attackRange_y = 0;
                damage = 0;
                break;
            case 1:
                attackRange_x = 5;
                attackRange_y = 2;
                damage = 10;
                break;
            case 3:
                attackRange_x = 5;
                attackRange_y = 2;
                damage = 15;
                break;
            case 4:
                animator.runtimeAnimatorController = overrideController_Level4;
                damage = 20;
                attackRange_x = 5.5f;
                attackRange_y = 2.5f;
                break;
            case 5:
                animator.runtimeAnimatorController = overrideController_Level4;
                damage = 25;
                attackRange_x = 5.5f;
                attackRange_y = 2.5f;
                break;
            case 6:
                animator.runtimeAnimatorController = overrideController_Level6;
                attackRange_x = 6;
                attackRange_y = 3f;
                damage = 30;
                break;
            case 7:
                animator.runtimeAnimatorController = overrideController_Level6;
                attackRange_x = 6;
                attackRange_y = 3f;
                damage = 35;
                break;
            case 8:
                animator.runtimeAnimatorController = overrideController_Level6;
                attackRange_x = 6;
                attackRange_y = 3f;
                damage = 40;
                break;
            default:
                break;
        }
    }
}
