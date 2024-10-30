using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;  // 싱글톤 인스턴스
    public static int maxHealth = 6; // �ִ� ü��
    public int currentHealth; // ���� ü��

    //쓸일 없으면 삭제.
    public int attackPower = 10; // ���ݷ�
    

    public GameObject[] Hp = new GameObject[maxHealth];
    public GameObject player;

    public static bool isMelee = false;

     private void Awake()
    {
        // 싱글톤 인스턴스를 설정합니다.
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // 인스턴스가 중복될 경우 삭제
            return;
        }

        DontDestroyOnLoad(gameObject); // 씬이 변경되더라도 인스턴스 유지
    }

    void Start()
    {
        currentHealth = maxHealth; // ���� ü���� �ִ� ü������ �ʱ�ȭ
    }

    // ���ظ� �Դ� �޼���
    public void TakeDamageToPlayer(int damage, string damageSource)
    {
        currentHealth -= damage;
        Debug.Log("현재 체력 : " + Hp[currentHealth]);
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
