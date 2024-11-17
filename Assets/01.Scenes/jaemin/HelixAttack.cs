using System.Collections;
using UnityEngine;

public class HelixAttack : MonoBehaviour
{
    public GameObject bulletPrefab; 
    public Transform firePoint;     
    public float bulletSpeed = 5f;  
    public float helixRadiusIncrement = 2f; // 반지름 증가 속도
    public float helixAngleSpeed = 200f; // 회전 속도 (각도/초)
    public float bulletLifetime = 5f; // 총알이 사라지는 시간
    public float fireRate = 0.2f; // 총알 발사 간격

    private bool isFiring = true; // 공격 활성화 상태

    void Start()
    {
        StartCoroutine(FireHelix()); // 잠시 멈추고 다시 실행하는 느낌
    }

    IEnumerator FireHelix() //코루틴 함수 반환할때는 IEnumerator 사용
    {
        while (isFiring)
        {
            SpawnBullet();
            yield return new WaitForSeconds(fireRate); // fireRate 만큼 실행을 잠시 멈춰주는 함수
        }
    }

    void SpawnBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // HelixBulletMovement 스크립트로 헬릭스 모양으로 이동
        HelixBulletMovement bulletMovement = bullet.AddComponent<HelixBulletMovement>();
        bulletMovement.Initialize(firePoint.position, bulletSpeed, helixRadiusIncrement, helixAngleSpeed, bulletLifetime);
    }
}