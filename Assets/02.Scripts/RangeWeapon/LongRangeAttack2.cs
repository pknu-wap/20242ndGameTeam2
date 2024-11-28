using UnityEngine;

public class LongRangeAttack2 : MonoBehaviour
{
    public GameObject bulletPrefab; // 총알 프리팹
    public Transform firePoint;     // 총알이 발사되는 위치
    public int weaponLevel = 0;     // 무기 레벨 (1: 기본, 2: 레벨업)
    public float bulletSpeed = 10f; // 총알 속도
    public float attackInterval = 1f; // 공격 간격 (초 단위)
    private float attackTimer = 0f;  // 타이머
    private int bulletCount;        // 현재 총알 개수
    private float[] bulletAngles;   // 현재 총알 각도 배열

    void Start()
    {
        weaponLevel = GameManager.Instance.longRangeAttack2_Level;
        UpdateWeaponStats(); // 초기 무기 상태 업데이트
    }

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
        foreach (float angle in bulletAngles) // 설정된 각도에 따라 발사
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

    // 무기 레벨에 따른 상태 업데이트
    public void UpdateWeaponStats()
    {
        switch (weaponLevel)
        {
            case 1:
                bulletCount = 3;
                bulletAngles = new float[] { 0f, 120f, 240f };
                break;
            case 2:
                bulletCount = 4;
                bulletAngles = new float[] { 0f, 90f, 180f, 270f };
                break;
            case 3:
                bulletCount = 5;
                bulletAngles = new float[] { 0f, 72f, 144f, 216f, 288f };
                break;
            case 4:
                bulletCount = 6;
                bulletAngles = new float[] { 0f, 60f, 120f, 180f, 240f, 300f };
                break;
            case 5:
                bulletCount = 7;
                bulletAngles = new float[] { 0f, 51f, 102f, 153f, 204f, 255f, 306f };
                break;
            case 6:
                bulletCount = 8;
                bulletAngles = new float[] { 0f, 45f, 90f, 135f, 180f, 225f, 270f, 315f };
                break;
            case 7:
                bulletCount = 9;
                bulletAngles = new float[] { 0f, 40f, 80f, 120f, 160f, 200f, 240f, 280f, 320f };
                break;
            case 8:
                bulletCount = 10;
                bulletAngles = new float[] { 0f, 36f, 72f, 108f, 144f, 180f, 216f, 252f, 288f, 324f };
                break;
        }
    }
}