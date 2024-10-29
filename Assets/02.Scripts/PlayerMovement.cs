using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;
    private float rollSpeed = 10f;
    public float rollDuration = 0.5f; // 구르기 지속 시간
    private bool isRolling = false;
    private Rigidbody2D rb;
    private Vector2 moveDirection;

    public Joystick joystick; // Joystick 연결
    public LayerMask wallLayer; // 벽 레이어 지정
    public float checkRadius = 0.3f; // 충돌을 검사할 반경
    public Vector2 checkOffset = new Vector2(0.2f, 0.2f); // 충돌 확인 위치 조절

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        ProcessInputs();
    }

    void FixedUpdate()
    {
        if (!isRolling)
        {
            Move();
        }
    }

    void ProcessInputs()
    {
        float moveX = joystick.Horizontal;
        float moveY = joystick.Vertical;
        moveDirection = new Vector2(moveX, moveY).normalized;
    }

    void Move()
    {
        // 이동할 위치를 계산
        Vector2 targetPosition = (Vector2)transform.position + moveDirection * moveSpeed * Time.fixedDeltaTime;

        // 이동하려는 방향에 따라 확인 위치 조정
        Vector2 adjustedPosition = targetPosition + checkOffset * moveDirection;

        // 이동할 위치에 벽이 있는지 OverlapCircle로 검사
        Collider2D hit = Physics2D.OverlapCircle(adjustedPosition, checkRadius, wallLayer);

        if (hit == null)
        {
            // 벽이 없을 때만 이동
            rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
        }
        else
        {
            // 벽이 있을 때는 이동을 멈춤
            rb.velocity = Vector2.zero;
        }
    }

    // 구르기 동작 실행
    public void Roll()
    {
        if (!isRolling)
        {
            StartCoroutine(PerformRoll());
        }
    }

    IEnumerator PerformRoll()
    {
        isRolling = true;
        Vector2 rollDirection = moveDirection;

        rb.velocity = rollDirection * rollSpeed;

        // 구르기 지속 시간만큼 대기
        yield return new WaitForSeconds(rollDuration);

        // 구르기 종료 후 원래 상태로 복귀
        isRolling = false;
    }

    // 충돌 범위를 시각적으로 확인하기 위해 Gizmos 사용
    void OnDrawGizmos()
    {
        Vector2 adjustedPosition = (Vector2)transform.position + checkOffset * moveDirection;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(adjustedPosition, checkRadius);
    }
}