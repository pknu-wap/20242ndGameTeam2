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

    private Collider2D[] enemyColliders; // 적의 콜라이더들 저장

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");//enemy 태그를 가진 적들을 찾고 배열에 포함(비활성화된 오브젝트에서는 못찾음)
        //이 배열을 초기화할 적당한 타이밍이 필요, 방 넘어갈때마다 갱신하거나 몬스터 소환 될 때마다 갱신하거나
        enemyColliders = new Collider2D[enemies.Length]; // Collider2D 배열 크기 설정

        // 각 적의 Collider2D 컴포넌트를 enemyColliders 배열에 저장
        for (int i = 0; i < enemies.Length; i++)
        {
            enemyColliders[i] = enemies[i].GetComponent<Collider2D>();
        }
    }

    void Update()
    {
        if (!isRolling)
        {
            ProcessInputs();
        }
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

        

        // 플레이어와 적 간의 충돌을 무시
        foreach (var enemy in enemyColliders)
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), enemy, true);
        }

        // 구르기 속도를 적용하여 움직임 시작
        float elapsedTime = 0f;
        while (elapsedTime < rollDuration)
        {
            rb.velocity = rollDirection * rollSpeed; // 저장된 rollDirection 사용
            elapsedTime += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }

        // 구르기 후, 적과의 충돌을 다시 활성화
        foreach (var enemy in enemyColliders)
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), enemy, false);
        }

        TutorialManager.isInvincible = false;
        isRolling = false;
    }
}
