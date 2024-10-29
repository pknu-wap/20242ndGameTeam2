using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushEnemy : MonoBehaviour
{
    public float speed; // 적 이동 속도
    public Rigidbody2D player;
    public float stopDistance; // 공격범위
    public GameObject incomingAlertSign; // IncomingAlertSign 프리팹
    public float chargeSpeed = 20f; // 돌진 속도
    public float alertTime = 1f; // 경고 표시 시간

    private bool isLive = true;
    private bool isCharging = false; // 돌진 중인지 여부
    private Rigidbody2D enemy;
    private Vector2 chargeDirection; // 돌진 방향
    private GameObject alert; // 경고 오브젝트

    void Awake()
    {
        enemy = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!isLive || isCharging)
            return;

        // 적이 플레이어를 향해 이동
        Vector2 dirVec = player.position - enemy.position;
        float distance = dirVec.magnitude;

        // 플레이어와 일정 거리 이상일 때만 이동
        if (distance > stopDistance)
        {
            Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
            enemy.MovePosition(enemy.position + nextVec);
        }
        else if (!isCharging && alert == null)
        {
            // 돌진 준비
            StartCoroutine(PrepareCharge(dirVec.normalized));
        }

        // 적의 속도를 0으로 고정
        enemy.velocity = Vector2.zero;
    }

    IEnumerator PrepareCharge(Vector2 direction)
    {
        // 적의 현재 위치를 저장
        Vector2 enemyPosition = enemy.position;

        // 경고 표시
        Vector2 alertPosition = enemyPosition + direction * 0.5f + new Vector2(0, 1f); // 고정된 위치
        alert = Instantiate(incomingAlertSign, alertPosition, Quaternion.identity);
        alert.transform.SetParent(transform); // 적 오브젝트에 붙이기

        yield return new WaitForSeconds(alertTime);

        // 경고 오브젝트 삭제
        Destroy(alert);

        // 돌진 시작
        isCharging = true;
        chargeDirection = direction; // 돌진할 방향 고정

        // 돌진을 FixedUpdate에서 수행하도록 코루틴 종료
        StartCoroutine(Charge());
    }

    IEnumerator Charge()
    {
        float chargeDuration = 1f; // 돌진 지속 시간
        float elapsedTime = 0f;

        while (elapsedTime < chargeDuration)
        {
            enemy.MovePosition(enemy.position + chargeDirection * chargeSpeed * Time.fixedDeltaTime);
            elapsedTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        isCharging = false; // 돌진 종료 후 대기 상태로 돌아감
        alert = null; // 경고 오브젝트 리셋
    }
}
