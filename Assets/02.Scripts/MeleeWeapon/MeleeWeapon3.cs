using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon3 : MonoBehaviour // 성경
{
    public GameObject bulletPrefab; // 총알 프리팹
    public float speed = 5f; // 회전 속도
    public float radius = 3f; // 원의 반지름
    private Transform playerTransform; // 플레이어의 Transform

    private GameObject bullet; // 생성된 총알 객체
    private float angle = 0f; // 회전 각도

    void Start()
    {
        // 플레이어의 Transform을 찾기
        playerTransform = GameObject.FindWithTag("Player").transform;

        // 총알을 플레이어의 자식으로 생성하고 z좌표는 -1로 설정
        bullet = Instantiate(bulletPrefab, new Vector3(playerTransform.position.x, playerTransform.position.y, -1f), Quaternion.identity);
        bullet.transform.SetParent(playerTransform); // 플레이어의 자식으로 설정
    }

    void Update()
    {
        // 각도를 증가시켜 회전
        angle += speed * Time.deltaTime;

        // 원을 그리기 위해 총알의 위치 계산
        Vector3 newBulletPosition = new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, -1f);

        // 총알이 플레이어를 중심으로 회전하도록 위치 설정
        bullet.transform.localPosition = newBulletPosition;

        // 총알이 회전하는 방향으로 회전하도록 설정
        bullet.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle * Mathf.Rad2Deg));
    }
}
