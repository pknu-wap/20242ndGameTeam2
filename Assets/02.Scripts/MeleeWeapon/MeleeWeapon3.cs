using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon3 : MonoBehaviour // 성경
{
    public GameObject bulletPrefab; // 총알 프리팹
    public float speed = 100f; // 회전 속도
    public float radius = 3f; // 원의 반지름
    private float angle = 0f; // 회전 각도
    public float bulletExistTime = 3f; // 총알 존재 시간 (초)
    public float bulletInactiveTime = 3f; // 총알 비활성 시간 (초)
    private Transform playerTransform; // 플레이어의 Transform
    private GameObject bullet; // 생성된 총알 객체

    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        StartCoroutine(SpawnBulletCycle()); // 총알 생성 사이클 시작
    }

    // 총알을 생성하고 3초 후에 삭제 후 다시 생성하는 코루틴
    IEnumerator SpawnBulletCycle()
    {
        while (true)
        {
            // 총알을 플레이어의 자식으로 생성하고 z좌표는 -1로 설정
            bullet = Instantiate(bulletPrefab, new Vector3(playerTransform.position.x, playerTransform.position.y, -1f), Quaternion.identity);
            bullet.transform.SetParent(playerTransform); // 플레이어의 자식으로 설정

            // 총알 존재 시간만큼 대기
            yield return new WaitForSeconds(bulletExistTime);

            // 총알 삭제
            Destroy(bullet);

            // 총알 비활성 시간만큼 대기
            yield return new WaitForSeconds(bulletInactiveTime);
        }
    }

    void Update()
    {
        if (bullet != null)
        {
            // 각도를 증가시켜서 회전
            angle += speed * Time.deltaTime;

            // 원을 그리기 위해 총알의 위치 계산
            Vector3 newBulletPosition = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, -1f);

            // 총알이 플레이어를 중심으로 회전하도록 위치 설정
            bullet.transform.localPosition = newBulletPosition;

            // 총알이 회전하는 방향으로 회전하도록 설정
            bullet.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle * Mathf.Rad2Deg));
        }
    }
}
