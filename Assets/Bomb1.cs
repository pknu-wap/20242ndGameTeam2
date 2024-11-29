using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb1 : MonoBehaviour
{
    private bool hasCollided = false;
    void Awake()
    {
        StartCoroutine(isBomb());
    }

    private IEnumerator isBomb()
    {
        
        yield return new WaitForSeconds(1.2f);
        DealDamageIfCollided();
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

    public void DealDamageIfCollided()
    {

        if (hasCollided)
        {
            Debug.Log("으악!");
            GameManager.Instance.TakeDamageToPlayer(1, "폭탄");
        }
    }
}
