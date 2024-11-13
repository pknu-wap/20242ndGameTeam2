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
            // 근접 공격 모드일 때만 발사
            while (GameManager.isMelee)
            {
                Vector2 direction = (playerTransform.position - arrowPos.position).normalized;

                GameObject instantArrow = Instantiate(arrow, arrowPos.position, Quaternion.FromToRotation(Vector3.right, -direction));

                Rigidbody2D rb = instantArrow.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.velocity = (-direction) * speed;
                }

                yield return new WaitForSeconds(rate);
            }

            yield return null; // `isMelee`가 `false`일 때 매 프레임 대기
        }
    }

}
