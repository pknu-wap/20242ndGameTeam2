using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeWeapon3 : MonoBehaviour // 성경
{
    public GameObject bulletPrefab; // 총알 프리팹
    [SerializeField] public int currentLevel = 0; // 현재 무기 레벨
    [SerializeField] private int baseDamage = 10; // 기본 공격력
    [SerializeField] private float speed = 100f; // 회전 속도
    [SerializeField] private float radius = 3f; // 원의 반지름
    [SerializeField] private float bulletExistTime = 3f; // 총알 존재 시간 (초)
    [SerializeField] private float bulletInactiveTime = 3f; // 총알 비활성 시간 (초)
    private Transform playerTransform; // 플레이어의 Transform
    [SerializeField] private List<GameObject> bullets = new List<GameObject>(); // 생성된 총알들
    private int bulletCount = 1; // 현재 투사체 수

    void Start()
    {
        currentLevel = GameManager.Instance.meleeWeapon3_Level;
        playerTransform = GameObject.FindWithTag("Player").transform;
        UpdateWeaponStats(); // 초기 무기 상태 업데이트
        StartCoroutine(SpawnBulletCycle()); // 총알 생성 사이클 시작
    }

    // 무기 레벨에 따른 상태 업데이트
    public void UpdateWeaponStats()
    {
        switch (currentLevel)
        {
            case 1:
                bulletCount = 1;
                break;
            case 2:
                bulletCount = 2;
                break;
            case 3:
                speed += 30f;
                radius += 1.25f;
                break;
            case 4:
                bulletExistTime += 0.5f;
                break;
            case 5:
                bulletCount = 3;
                break;
            case 6:
                speed += 39f; // 회전 속도 증가
                radius += 1.5625f; // 반지름 증가
                break;
            case 7:
                bulletExistTime += 0.5f;
                break;
            case 8:
                bulletCount = 4;
                break;
        }

        foreach (GameObject bullet in bullets)
        {
            if (bullet != null)
            {
                MeleeWeapon3Bullet bulletScript = bullet.GetComponent<MeleeWeapon3Bullet>();
                if (bulletScript != null)
                {
                    bulletScript.baseDamage = baseDamage; // 데미지 동기화
                }
            }
        }
    }


    // 총알 생성 및 관리 코루틴
    IEnumerator SpawnBulletCycle()
    {
        while (true)
        {
            SpawnBullets();

            yield return new WaitForSeconds(bulletExistTime);

            ClearBullets();

            yield return new WaitForSeconds(bulletInactiveTime);
        }
    }

    // 총알 생성 메서드
    void SpawnBullets()
    {
        ClearBullets(); // 기존 총알 삭제
        float angleStep = 360f / bulletCount; // 각 투사체의 각도 간격

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 spawnPosition = new Vector3(
                Mathf.Cos(angle) * radius,
                Mathf.Sin(angle) * radius,
                -1f
            );

            GameObject bullet = Instantiate(bulletPrefab, playerTransform.position + spawnPosition, Quaternion.identity, playerTransform);
            bullets.Add(bullet);
        }
    }

    // 총알 삭제 메서드
    void ClearBullets()
    {
        foreach (GameObject bullet in bullets)
        {
            if (bullet != null)
                Destroy(bullet);
        }
        bullets.Clear();
    }

    void Update()
    {
        if (bullets.Count > 0)
        {
            float angleStep = 360f / bulletCount;
            for (int i = 0; i < bullets.Count; i++)
            {
                float angle = (i * angleStep + Time.time * speed) * Mathf.Deg2Rad;

                Vector3 newBulletPosition = new Vector3(
                    Mathf.Cos(angle) * radius,
                    Mathf.Sin(angle) * radius,
                    -1f
                );

                bullets[i].transform.localPosition = newBulletPosition;
                bullets[i].transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle * Mathf.Rad2Deg));
            }
        }
    }
}
