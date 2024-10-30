using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float rate;                       // 발사 간격
    public float speed;                      // 발사 속도
    public Transform arrowPos;               // 화살 발사 위치 (Transform)
    public GameObject arrow;                 // 화살 프리팹
    public Transform playerTransform;        // 플레이어의 Transform

    private void Start()
    {
        // 화살을 일정 간격으로 발사하도록 코루틴 시작
        StartCoroutine(AutoFire());
    }

    IEnumerator AutoFire()
    {
        while (true)
        {
            // 플레이어의 이동 방향을 계산하고 반전
            Vector2 direction = (playerTransform.position - arrowPos.position).normalized;

            // 화살 생성 (화살이 플레이어의 이동 방향의 반대로 바라보도록 회전)
            GameObject instantArrow = Instantiate(arrow, arrowPos.position, Quaternion.FromToRotation(Vector3.right, -direction));

            // 화살에 속도 적용
            Rigidbody2D rb = instantArrow.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = (-direction) * speed; // 반전된 방향으로 발사 속도 적용
            }

            // 발사 간격만큼 대기
            yield return new WaitForSeconds(rate);
        }
    }
}
