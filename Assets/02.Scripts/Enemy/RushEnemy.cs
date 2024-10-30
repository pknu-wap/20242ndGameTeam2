using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushEnemy : MonoBehaviour
{
    [SerializeField] private float speed; // �� �̵� �ӵ�
    [SerializeField] private float stopDistance; // ���ݹ���
    [SerializeField] private float chargeSpeed; // ���� �ӵ�
    [SerializeField] private float alertTime; // ��� ǥ�� �ð�
    [SerializeField] private float waitAfterChargeTime; // ���� �� ��� �ð�
    [SerializeField] private Rigidbody2D player;
    [SerializeField] private GameObject incomingAlertSign; // IncomingAlertSign ������

    private bool isLive = true; // ����ִ��� ����
    private bool isCharging = false; // ���� ������ ����
    private bool isPreparingCharge = false; // ���� �غ� ������ ����
    private bool isWaitingAfterCharge = false; // ���� �� ��� ���� ����
    [SerializeField] private bool isCollision = false; // �浹 ����
    private Rigidbody2D enemy;
    private Vector2 chargeDirection; // ���� ����
    private GameObject alert; // ��� ������Ʈ

    void Awake()
    {
        enemy = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!isLive || isCharging || isPreparingCharge || isWaitingAfterCharge)
            return;

        // ���� �÷��̾ ���� �̵�
        Vector2 dirVec = player.position - enemy.position;
        float distance = dirVec.magnitude;

        // �÷��̾�� ���� �Ÿ� �̻��� ���� �̵�
        if (distance > stopDistance)
        {
            Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
            enemy.MovePosition(enemy.position + nextVec);
        }
        else if (!isCharging && alert == null)
        {
            // ���� �غ�
            StartCoroutine(PrepareCharge(dirVec.normalized));
        }

        // ���� �ӵ��� 0���� ����
        //enemy.velocity = Vector2.zero;
    }

    IEnumerator PrepareCharge(Vector2 direction)
    {
        // ���� �غ� ���� - ���� �̵� ����
        isPreparingCharge = true;

        // ���� ���� ��ġ�� ����
        Vector2 enemyPosition = enemy.position;

        // ��� ǥ�� - ���� ��ġ�� �������� ������ ��ġ�� ����
        Vector2 alertPosition = enemyPosition + new Vector2(0, 2f); // ���� ��ġ �ٷ� ���� ǥ��
        alert = Instantiate(incomingAlertSign, alertPosition, Quaternion.identity);
        alert.transform.SetParent(transform); // �� ������Ʈ�� ���̱�

        yield return new WaitForSeconds(alertTime);

        // ��� ������Ʈ ����
        Destroy(alert);

        // ���� ����
        isCharging = true;
        chargeDirection = direction; // ������ ���� ����
        isPreparingCharge = false; // ���� �غ� �Ϸ� - �̵� ���� ���·� ����

        // ������ FixedUpdate���� �����ϵ��� �ڷ�ƾ ����
        StartCoroutine(Charge());
    }

    IEnumerator Charge()
    {
        float chargeDuration = 1f; // ���� ���� �ð�
        float elapsedTime = 0f; // ���� ��� �ð�

        isCollision = false;

        while (elapsedTime < chargeDuration)
        {
            if (isCollision)
            {
                break;
            }
            /*enemy.velocity�� ���߿� ���� */
            enemy.MovePosition(enemy.position + chargeDirection * chargeSpeed * Time.fixedDeltaTime);
            elapsedTime += Time.fixedDeltaTime;
            Debug.Log("코루틴시작");
            yield return new WaitForFixedUpdate();
        }

        alert = null; // ��� ������Ʈ ����
        isCharging = false; // ���� ���� �� ��� ���·� ���ư�
        isCollision = false;

        // ���� �� ��� ���·� ��ȯ
        isWaitingAfterCharge = true;
        yield return new WaitForSeconds(waitAfterChargeTime);
        isWaitingAfterCharge = false; // ��� �ð� ���� �� �̵� ���� ���·� ����
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isCollision = true;
    }
}
