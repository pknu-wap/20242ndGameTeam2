using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed;
    // 적이 죽을 때 추가되는 경험치
    [SerializeField] private int expOnDeath;
    public Rigidbody2D player;

    bool isLive = true;

    Rigidbody2D enemy;

    void Awake()
    {
        enemy = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // 적이 죽은 상태라면 작동 X
        if (!isLive)
            return;
        // 목표의 위치와 현재 적의 위치를 빼서 방향 벡터 설정
        Vector2 dirVec = player.position - enemy.position;
        // dirVec을 normalized하는 이유는 모든 방향의 길이를 정규화해놓아야 이동 속도가 같아지기 때문이다.
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
        // 적을 nextVec만큼 이동
        enemy.MovePosition(enemy.position + nextVec);
        enemy.velocity = Vector2.zero;
    }

    // 적이 죽을 때 호출되는 함수
    public void Die()
    {
        if (!isLive)
            return;

        isLive = false; // 적이 죽었으므로 isLive를 false로 설정

        // 경험치 증가
        GameManager.Instance.AddExperience(expOnDeath);

        // 적을 삭제하거나 비활성화하는 코드 추가 (예: 적 오브젝트 비활성화)
        gameObject.SetActive(false); // 적이 죽으면 비활성화
        Debug.Log("적 처치! 경험치 + " + expOnDeath);
    }
}
