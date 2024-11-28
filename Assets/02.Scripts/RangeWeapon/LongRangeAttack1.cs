using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongRangeAttack1 : MonoBehaviour
{
    public GameObject bullet;
    public Transform firePoint;
    public float bulletSpeed = 10f;
    public float attackRange = 5f;
    public float baseAttackCooldown = 1f; // 기본 쿨다운
    public int weaponLevel = 1; // 무기 레벨

    private float lastAttackTime;
    private float attackCooldown; // 현재 공격 쿨다운
    private Scanner scanner;

    private void Start()
    {
        scanner = GetComponent<Scanner>();
        UpdateAttackCooldown(); // 시작 시 쿨다운 설정
    }

    void Update()
    {
        Transform target = scanner.nearestTarget;

        if (target != null && Time.time >= lastAttackTime + attackCooldown && GameManager.isMelee == false)
        {
            Shoot(target);
            lastAttackTime = Time.time;
        }
    }

    void Shoot(Transform target)
    {
        Vector2 targetPosition = target.position;

        // firePoint 기준으로 발사 위치를 계산
        List<Vector2> firePositions = CalculateFirePositions();

        foreach (Vector2 offset in firePositions)
        {
            Vector2 spawnPosition = (Vector2)firePoint.position + offset;
            GameObject bullet_shoot = Instantiate(bullet, spawnPosition, Quaternion.identity);
            Rigidbody2D rb = bullet_shoot.GetComponent<Rigidbody2D>();

            Vector2 direction = (targetPosition - spawnPosition).normalized;
            rb.velocity = direction * bulletSpeed;
        }
    }

    public List<Vector2> CalculateFirePositions()
    {
        List<Vector2> positions = new List<Vector2>();

        switch (weaponLevel)
        {
            case 1:
                positions.Add(Vector2.up * 0.5f); // 머리 위
                break;

            case 2:
                positions.Add(Quaternion.Euler(0, 0, -45) * Vector2.right * 0.5f); // -45도
                positions.Add(Quaternion.Euler(0, 0, 45) * Vector2.right * 0.5f);  // 45도
                break;

            case 3:
                positions.Add(Quaternion.Euler(0, 0, -45) * Vector2.right * 0.5f); // -45도
                positions.Add(Vector2.up * 0.5f);                                // 0도
                positions.Add(Quaternion.Euler(0, 0, 45) * Vector2.right * 0.5f);  // 45도
                break;

            case 4:
                positions.Add(Quaternion.Euler(0, 0, -60) * Vector2.right * 0.5f); // -60도
                positions.Add(Quaternion.Euler(0, 0, -30) * Vector2.right * 0.5f); // -30도
                positions.Add(Quaternion.Euler(0, 0, 30) * Vector2.right * 0.5f);  // 30도
                positions.Add(Quaternion.Euler(0, 0, 60) * Vector2.right * 0.5f);  // 60도
                break;

            case 5:
            case 6: // 6레벨부터는 총알 개수 유지
            case 7:
            case 8:
                positions.Add(Quaternion.Euler(0, 0, -60) * Vector2.right * 0.5f); // -60도
                positions.Add(Quaternion.Euler(0, 0, -30) * Vector2.right * 0.5f); // -30도
                positions.Add(Vector2.up * 0.5f);                                // 0도
                positions.Add(Quaternion.Euler(0, 0, 30) * Vector2.right * 0.5f);  // 30도
                positions.Add(Quaternion.Euler(0, 0, 60) * Vector2.right * 0.5f);  // 60도
                break;

            default:
                positions.Add(Vector2.up * 0.5f); // 기본값: 머리 위
                break;
        }

        return positions;
    }

    public void UpdateAttackCooldown()
    {
        // 6~8레벨에서는 연사 속도 증가
        switch (weaponLevel)
        {
            case 6:
                attackCooldown = baseAttackCooldown * 0.75f; // 쿨다운 25% 감소
                break;

            case 7:
                attackCooldown = baseAttackCooldown * 0.5f; // 쿨다운 50% 감소
                break;

            case 8:
                attackCooldown = baseAttackCooldown * 0.3f; // 쿨다운 70% 감소
                break;

            default:
                attackCooldown = baseAttackCooldown; // 기본 쿨다운
                break;
        }
    }

    // 무기 레벨이 변경되었을 때 호출
    public void SetWeaponLevel(int newLevel)
    {
        weaponLevel = newLevel;
        UpdateAttackCooldown();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}