using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    /*접근 지정자 or 접근 제한자
    public , private, protected 공부해오기

    유니티 물리 rigidbody2D
    Body Type에 다이나믹 키네마틱 스테틱 차이 알아오기 */
    public float moveSpeed = 5f;
    public float rollSpeed = 10f;    // 구르기 시 속도
    public float rollDuration = 0.5f; // 구르기 지속 시간
    private bool isRolling = false;
    private Rigidbody2D rb;
    private Vector2 moveDirection;

    public Joystick joystick;        // Joystick 연결
    public GameObject rollButton;    // 구르기 버튼 연결

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // 구르기 버튼 클릭 시 Roll() 메서드 호출
        rollButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(Roll);
    }

    void Update()
    {
        ProcessInputs();
    }

    void FixedUpdate()
    {
        Move();
    }

    void ProcessInputs()
    {
        float moveX = joystick.Horizontal;
        float moveY = joystick.Vertical;
        moveDirection = new Vector2(moveX, moveY).normalized;
    }

    void Move()
    {
        if (!isRolling)
        {
            rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
        }
    }

    // 구르기 동작 실행
    void Roll()
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

        // 구르기 동안 속도 증가
        rb.velocity = rollDirection * rollSpeed;

        // 구르기 지속 시간만큼 대기
        yield return new WaitForSeconds(rollDuration);

        // 구르기 종료 후 원래 속도로 복귀
        isRolling = false;
    }
}