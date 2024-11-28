using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class IcicleDamage : BaseEnemy
{
    private bool hasCollided = false; // 충돌 여부 체크

    // 충돌 시 데미지를 처리하는 함수
    protected override void Awake()
    {
        StartCoroutine(IcicleDestroy());
    }
    private IEnumerator IcicleDestroy()
    {
        yield return new WaitForSeconds(4f);
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어와 충돌했을 때
        if (other.CompareTag("Player"))
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
            GameManager.Instance.TakeDamageToPlayer(1,"얼음공격");
        }
    }
}

