using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static int maxHealth = 6; // �ִ� ü��
    public int currentHealth; // ���� ü��

    //쓸일 없으면 삭제.
    public int attackPower = 10; // ���ݷ�
    
    
    public GameObject[] Hp = new GameObject[maxHealth];
    public GameObject player;

    public static bool isMelee = false;

    void Start()
    {
        currentHealth = maxHealth; // ���� ü���� �ִ� ü������ �ʱ�ȭ
    }

    // ���ظ� �Դ� �޼���
    public void TakeDamage(int damage, string damageSource)
    {
        currentHealth -= damage;
        Hp[currentHealth].SetActive(false);
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // ���� ü���� 0 ���Ϸδ� �� �������� maxHealth���� Ŀ�� �� ������ Ŭ����

        if (currentHealth <= 0)
        {
            Die(); // ü���� 0 ���ϰ� �Ǹ� ���
            
        }
    }

    // ��� ó�� �޼���
    private void Die()
    {
        player.SetActive(false);
        // ��� �� ó���� ���� (��: ���� ���� ȭ�� ǥ�� ��)
    }

    // ���� �޼��� (�ʿ��� ���)
    public void Attack(GameObject target)
    {
        // ���� ��� ���ظ� ������ ���� ����
        // ��: target.GetComponent<Enemy>().TakeDamage(attackPower);
    }

    public void WeaponChange()
    {
        if (isMelee == true)
        { 
            isMelee = false;
        }
        else //원거리공격일때 근접공격으로 전환
        {
            isMelee = true;
        }
    }
}
