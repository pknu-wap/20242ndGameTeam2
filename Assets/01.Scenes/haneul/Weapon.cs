using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float rate;                       // 발사 간격
    public float speed;                      // 발사 속도
    public Transform arrowPos;               // 화살 발사 위치 (Transform)
    public GameObject arrow;                 // 화살 프리팹

    private void Start()
    {
        // 화살을 일정 간격으로 발사하도록 코루틴 시작
        StartCoroutine(AutoFire());
    }

    IEnumerator AutoFire()
    {
        while (true)
        {
            // 화살 생성
            GameObject instantArrow = Instantiate(arrow, arrowPos.position, arrowPos.rotation);

            // 화살에 속도 적용 (앞 방향으로 발사)
            Rigidbody2D rb = instantArrow.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = transform.right * speed; // rate를 화살 속도로 활용
            }

            // 발사 간격만큼 대기 (3초)
            yield return new WaitForSeconds(rate);
        }
    }
}
