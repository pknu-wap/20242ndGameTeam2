using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandDamage : MonoBehaviour
{
    private bool hasCollided = false; // 충돌 여부 체크

    void Awake()
    {
        StartCoroutine(HandDestroy());
    }

    private IEnumerator HandDestroy()
    {
        yield return new WaitForSeconds(2.35f);

        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어와 충돌했을 때
        if (other.CompareTag("Player") && !GameManager.isInvincible)
        {

            hasCollided = true; // 충돌 처리 상태로 설정
        }

        else
        {
            hasCollided = false;
        }
    }

    // 애니메이션 이벤트에서 호출할 함수
    public void DealDamageIfCollided()
    {

        if (hasCollided)
        {
            Debug.Log("으악!");
            GameManager.Instance.TakeDamageToPlayer(1, "손아귀");
        }
    }
}
