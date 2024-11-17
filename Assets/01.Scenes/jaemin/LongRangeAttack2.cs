using UnityEngine;

public class LongRangeAttack2 : MonoBehaviour
{
    public GameObject bulletPrefab; // 총알 프리팹
    public Transform firePoint;     // 총알이 발사되는 위치
    public int weaponLevel = 2;     // 무기 레벨 (1: 기본, 2: 레벨업)
    public float bulletSpeed = 10f; // 총알 속도
    public float attackInterval = 1f; // 공격 간격 (초 단위)
    private float attackTimer = 0f;  // 타이머

    void Update()
    {
        // 공격 딜레이
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackInterval)
        {
            Fire(); 
            attackTimer = 0f; 
        }
    }

    public void Fire()
    {
        int bulletCount = GetBulletCount();
        float[] angles = GetBulletAngles(bulletCount); // 각도를 미리 설정

        foreach (float angle in angles) // 각도 가져오기
        {
            // 각도를 회전 값으로 변환
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation * rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = bullet.transform.up * bulletSpeed; // 총알 발사 방향 설정
            }
        }
    }

    // 무기 레벨에 따른 총알 개수 반환
    private int GetBulletCount()
    {
        switch (weaponLevel)
        {
            case 1: return 3; // 레벨 1: 3발
            case 2: return 4; // 레벨 2: 4발
            case 3: return 5; // 레벨 3: 5발 - (최대 레벨 정하고 늘리면 될듯)
            case 4: return 6;
            case 5: return 7;
            case 6: return 8;
            default: return 3; // 기본값
        }
    }

    // 무기 레벨에 따른 각도 범위 반환
    private float[] GetBulletAngles(int bulletCount)
    {
        if (bulletCount == 3)
        {
            return new float[] { 0f, 120f, 240f }; // 3발: 0도, 120도, 240도
        }
        else if (bulletCount == 4)
        {
            return new float[] { 0f, 90f, 180f, 270f }; // 4발: 0도, 90도, 180도, 270도
        }
        else if (bulletCount == 5)
        {
            return new float[] { 0f, 72f, 144f, 216f, 288f }; // 5발: 5개로 나눠서 발사
        }
        else if (bulletCount == 6)
        {
            return new float[] { 0f, 60f, 120f, 180f, 240f, 300f };
        }
        else if (bulletCount == 7)
        {
            return new float[] { 0f, 51f, 102f, 153f, 204f, 255f, 306f };
        }
        else if (bulletCount == 8)
        {
            return new float[] { 0f, 45f, 90f, 135f, 180f, 225f, 270f, 315f };
        }
        return new float[] { 0f }; // 기본값
    }
}