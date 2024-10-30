using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int maxHealth = 100; // 최대 체력
    public int currentHealth; // 현재 체력
    public int attackPower = 10; // 공격력

    void Start()
    {
        currentHealth = maxHealth; // 현재 체력을 최대 체력으로 초기화
    }

    // 피해를 입는 메서드
    public void TakeDamage(int damage, string damageSource)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // 현재 체력이 0 이하로는 못 떨어지게 maxHealth보다 커질 수 없도록 클램프

        if (currentHealth <= 0)
        {
            Die(); // 체력이 0 이하가 되면 사망
        }
    }

    // 사망 처리 메서드
    private void Die()
    {
        Debug.Log("플레이어 사망");
        // 사망 시 처리할 로직 (예: 게임 오버 화면 표시 등)
    }

    // 공격 메서드 (필요한 경우)
    public void Attack(GameObject target)
    {
        // 공격 대상에 피해를 입히는 로직 구현
        // 예: target.GetComponent<Enemy>().TakeDamage(attackPower);
    }
}
