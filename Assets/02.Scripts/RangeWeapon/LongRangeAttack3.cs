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

    private int projectileCount = 1;         // 투사체 개수
    public static int pierceCount = 0;       // 관통 수
    public int Level = 0;

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

    public void UpdateStatsByLevel()
    {
        Level = GameManager.Instance.longRangeAttack3_Level;
        switch (Level)
        {
            case 0:
                break;
            case 1:
                rate = 1f;
                arrowDamage = 10;
                projectileCount = 1;
                pierceCount = 0;
                break;
            case 2:
                projectileCount = 2; // 투사체 +1
                break;
            case 3:
                projectileCount = 3; // 투사체 +1
                arrowDamage = 15;    // 공격력 +5
                break;
            case 4:
                projectileCount = 4; // 투사체 +1
                rate = 0.96f;        // 쿨타임 -0.04초
                break;
            case 5:
                pierceCount = 1;     // 관통 수 +1
                break;
            case 6:
                projectileCount = 5; // 투사체 +1
                rate = 0.92f;        // 쿨타임 -0.04초
                break;
            case 7:
                projectileCount = 6; // 투사체 +1
                arrowDamage = 20;    // 공격력 +5
                break;
            case 8:
                pierceCount = 2;     // 관통 수 +1
                rate = 0.88f;        // 쿨타임 -0.04초
                break;
        }
    }

    IEnumerator AutoFire()
    {
        while (true)
        {
            UpdateStatsByLevel();
            // 근접 공격 모드가 아닐 때만 발사
            while (!GameManager.isMelee && Level > 0 && !GameManager.isMelee)
            {
                
                for (int i = 0; i < projectileCount; i++)
                {
                    // 각 화살의 발사 위치를 조금씩 다르게 설정 (살짝 위 또는 아래로 발사)
                    float offset = (i % 2 == 0) ? 0.5f : -0.5f; // 한쪽은 위로, 다른 한쪽은 아래로

                    // 발사 위치를 변경하여 화살이 살짝 위 또는 아래로 발사됨
                    Vector3 spawnPosition = arrowPos.position + new Vector3(0, offset, 0); // Y축으로 오프셋 추가

                    // 투사체 생성
                    GameObject instantArrow = Instantiate(arrow, spawnPosition, Quaternion.identity);

                    // 발사 방향 계산 (화살이 가는 방향)
                    Vector2 direction = (playerTransform.position - arrowPos.position).normalized;

                    // 원래의 발사 각도를 유지하는 부분 (Quaternion.FromToRotation 사용)
                    Quaternion rotation = Quaternion.FromToRotation(Vector3.right, -direction);
                    instantArrow.transform.rotation = rotation; // 기존의 발사 각도를 그대로 적용

                    // Rigidbody2D에 속도 설정 (화살이 방향을 따라 발사됨)
                    Rigidbody2D rb = instantArrow.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        rb.velocity = (-direction) * speed; // 발사 방향으로 속도 설정
                    }

                    // 관통 수 처리 (예: 화살에 `Piercing` 스크립트가 있다고 가정)
                    if (pierceCount > 0)
                    {
                        var piercing = instantArrow.GetComponent<LongRangeNonTargeting>();
                        if (piercing != null)
                        {
                            piercing.SetPierceCount(pierceCount);
                        }
                    }
                    yield return new WaitForSeconds(0.12f);
                }

                yield return new WaitForSeconds(rate-0.12f*projectileCount); // 발사 간격 설정
            }

            yield return null; // `isMelee`가 `true`일 때 매 프레임 대기
        }
    }
}
