using System.Collections;
using UnityEngine;

public class HelixAttack : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 5f;
    public float helixRadiusIncrement = 2f; // 반지름 증가 속도
    public float helixAngleSpeed = 200f;    // 회전 속도 (각도/초)
    public float bulletLifetime = 5f;       // 총알이 사라지는 시간
    public float baseFireRate = 0.5f;       // 기본 발사 간격
    public int weaponLevel = 1;             // 무기 레벨

    private float fireRate;
    private bool isFiring = true;

    void Start()
    {
        UpdateFireRate(); // 초기화
        StartCoroutine(FireHelix());
    }

    IEnumerator FireHelix()
    {
        while (isFiring)
        {
            FireBullets(); // 레벨에 따라 총알 발사
            yield return new WaitForSeconds(fireRate);
        }
    }

    void FireBullets()
    {
        int bulletCount = GetBulletCountByLevel();

        for (int i = 0; i < bulletCount; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

            // HelixBulletMovement 스크립트를 추가하여 헬릭스 이동 설정
            HelixBulletMovement bulletMovement = bullet.AddComponent<HelixBulletMovement>();
            bulletMovement.Initialize(transform, bulletSpeed, helixRadiusIncrement, helixAngleSpeed, bulletLifetime, i, bulletCount);
        }
    }

    int GetBulletCountByLevel()
    {
        // 1~5 레벨: 발사체 증가 / 6~8 레벨: 발사체 고정 (5개)
        if (weaponLevel >= 6)
            return 5;

        switch (weaponLevel)
        {
            case 1: return 1;
            case 2: return 2;
            case 3: return 3;
            case 4: return 4;
            case 5: return 5;
            default: return 1; // 기본값
        }
    }

    void UpdateFireRate()
    {
        // 6~8 레벨에서 연사속도 증가
        switch (weaponLevel)
        {
            case 6:
                fireRate = baseFireRate * 0.75f; // 쿨다운 25% 감소
                break;
            case 7:
                fireRate = baseFireRate * 0.5f; // 쿨다운 50% 감소
                break;
            case 8:
                fireRate = baseFireRate * 0.3f; // 쿨다운 70% 감소
                break;
            default:
                fireRate = baseFireRate;
                break;
        }
    }

    public void SetWeaponLevel(int level)
    {
        weaponLevel = level;
        UpdateFireRate(); // 무기 레벨 변경 시 발사 속도 업데이트
    }
}