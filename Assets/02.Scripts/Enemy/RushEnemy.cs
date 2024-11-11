using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushEnemy : BaseEnemy
{
    [SerializeField] private float speed; // 이동 속도
    [SerializeField] private float stopDistance; // 정지 거리
    [SerializeField] private float chargeSpeed; // 충전 속도
    [SerializeField] private float alertTime; // 경고 시간
    [SerializeField] private float waitAfterChargeTime; // 충전 후 대기 시간
    [SerializeField] private Rigidbody2D player;
    [SerializeField] private GameObject incomingAlertSign; // IncomingAlertSign 오브젝트
    [SerializeField] private int damageAmount = 1;

    private bool isLive = true; // 살아있는지 여부
    private bool isCharging = false; // 충전 중인지 여부
    private bool isPreparingCharge = false; // 충전 준비 중인지 여부
    private bool isWaitingAfterCharge = false; // 충전 후 대기 중인지 여부
    private bool isCollision = false; // 충돌 여부
    private bool isPlayer = false; // 플레이어와 충돌했는지 여부
    private Rigidbody2D enemy;
    private Vector2 chargeDirection; // 충전 방향
    private GameObject alert; // 경고 아이콘

    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponent<Rigidbody2D>();
        damageMultiplier = 1.0f;
        expOnDeath = 100;
    }

    private void FixedUpdate()
    {
        if (!isLive || isCharging || isPreparingCharge || isWaitingAfterCharge)
            return;

        // 플레이어와의 거리 계산
        Vector2 dirVec = player.position - enemy.position;
        float distance = dirVec.magnitude;

        if (distance > stopDistance)
        {
            // 플레이어와의 거리가 멀면 이동
            Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
            enemy.MovePosition(enemy.position + nextVec);
        }
        else if (!isCharging && alert == null)
        {
            // 가까워지면 충전 준비 시작
            StartCoroutine(PrepareCharge(dirVec.normalized));
        }
    }

    IEnumerator PrepareCharge(Vector2 direction)
    {
        isPreparingCharge = true;

        // 경고 아이콘 생성
        Vector2 enemyPosition = enemy.position;
        Vector2 alertPosition = enemyPosition + new Vector2(0, 2f); // 적의 위쪽에 경고 아이콘 배치
        alert = Instantiate(incomingAlertSign, alertPosition, Quaternion.identity);
        alert.transform.SetParent(transform);

        yield return new WaitForSeconds(alertTime);

        // 경고 아이콘 삭제
        Destroy(alert);

        isCharging = true;
        chargeDirection = direction; // 충전 방향 설정
        isPreparingCharge = false;

        StartCoroutine(Charge());
    }

    IEnumerator Charge()
    {
        float chargeDuration = 1f;
        float elapsedTime = 0f;

        isCollision = false;

        while (elapsedTime < chargeDuration)
        {
            if (isCollision)
            {
                if (isPlayer)
                {
                    isPlayer = false;
                    GameManager.Instance.TakeDamageToPlayer(damageAmount, "RushEnemy 충돌");
                }
                break;
            }

            enemy.MovePosition(enemy.position + chargeDirection * chargeSpeed * Time.fixedDeltaTime);
            elapsedTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        alert = null;
        isCharging = false;

        // 충전 후 잠시 대기
        isWaitingAfterCharge = true;
        yield return new WaitForSeconds(waitAfterChargeTime);
        isWaitingAfterCharge = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isCollision = true;
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayer = true;
        }
    }

    protected override void Die()
    {
        // RushEnemy의 사망 처리
        base.Die(); // BaseEnemy의 Die 메서드를 호출하여 경험치를 추가하고, 기본적인 사망 처리
    }
}
