using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;
    public float rollSpeed = 10f;    // 구르기 시 속도
    public float rollDuration = 0.5f; // 구르기 지속 시간
    private bool isRolling = false;
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private Vector2 rollDirection; // 구르기 방향 저장

    public Joystick joystick;        // Joystick 연결

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
        Move();
    }

    void ProcessInputs()
    {
        float moveX = joystick.Horizontal;
        float moveY = joystick.Vertical;
        moveDirection = new Vector2(moveX, moveY).normalized;

        if (moveX != 0 && moveY != 0)
        {
            rollDirection = moveDirection; // 구르기 방향 저장
        }
    }

    void Move()
    {
        if (!isRolling)
        {
            rb.velocity = new Vector2(moveDirection.x * moveSpeed, moveDirection.y * moveSpeed);
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
        TutorialManager.isInvincible = true;
        // 구르기 속도를 적용하여 움직임 시작
        float elapsedTime = 0f;
        while (elapsedTime < rollDuration)
        {
            rb.velocity = rollDirection * rollSpeed; // 저장된 rollDirection 사용
            elapsedTime += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }
        TutorialManager.isInvincible = false;
        isRolling = false;

    }
}
