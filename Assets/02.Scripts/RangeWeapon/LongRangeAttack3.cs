using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongRangeAttack3 : MonoBehaviour
{
    private Vector2 moveDirection;
    public Joystick joystick;

    public float rate = 1;                   // 초기 발사 간격
    public float speed;                      // 발사 속도
    public Transform arrowPos;               // 화살 발사 위치 (Transform)
    public GameObject arrow;                 // 화살 프리팹
    public Transform playerTransform;        // 플레이어의 Transform
    public static int arrowDamage = 10;


    private void Start()
    {
        // 화살을 일정 간격으로 발사하도록 코루틴 시작
        StartCoroutine(AutoFire());
    }

    void Update()
    {
        ProcessInputs();
        MovePoint();
    }

    void ProcessInputs()
    {
        float moveX = joystick.Horizontal;
        float moveY = joystick.Vertical;

        moveDirection = new Vector2(moveX, moveY).normalized; // 입력 방향을 정규화
    }

    void MovePoint()
    {
        // 조이스틱 입력이 있을 때만 위치를 이동
        if (moveDirection != Vector2.zero)
        {
            // 플레이어 위치에서 moveDirection을 더하여 이동
            transform.position = playerTransform.position + (Vector3)moveDirection;
        }
    }

    IEnumerator AutoFire()
    {
        while (true)
        {
            rate = 1 - (GameManager.longRangeAttack3 * 0.05f);
            arrowDamage = 10 + GameManager.longRangeAttack3*5;
            // 근접 공격 모드가 아닐 때만 발사
            while (GameManager.isMelee == false)
            {
                Vector2 direction = (playerTransform.position - arrowPos.position).normalized;

                GameObject instantArrow = Instantiate(arrow, arrowPos.position, Quaternion.FromToRotation(Vector3.right, -direction));

                float scaleMultiplier = 1 + (GameManager.longRangeAttack3 / 2) * 0.15f;
                instantArrow.transform.localScale = new Vector3(scaleMultiplier, scaleMultiplier*0.25f, 1);

                Rigidbody2D rb = instantArrow.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.velocity = (-direction) * speed;
                }

                yield return new WaitForSeconds(rate);
            }

            yield return null; // `isMelee`가 `true`일 때 매 프레임 대기
        }
    }

}

