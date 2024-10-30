using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int maxHealth = 6; // �ִ� ü��
    public int currentHealth; // ���� ü��
    public int attackPower = 10; // ���ݷ�

    void Start()
    {
        currentHealth = maxHealth; // ���� ü���� �ִ� ü������ �ʱ�ȭ
    }

    // ���ظ� �Դ� �޼���
    public void TakeDamage(int damage, string damageSource)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // ���� ü���� 0 ���Ϸδ� �� �������� maxHealth���� Ŀ�� �� ������ Ŭ����

        if (currentHealth <= 0)
        {
            Die(); // ü���� 0 ���ϰ� �Ǹ� ���
        }
    }

    // ��� ó�� �޼���
    private void Die()
    {
        Debug.Log("�÷��̾� ���");
        // ��� �� ó���� ���� (��: ���� ���� ȭ�� ǥ�� ��)
    }

    // ���� �޼��� (�ʿ��� ���)
    public void Attack(GameObject target)
    {
        // ���� ��� ���ظ� ������ ���� ����
        // ��: target.GetComponent<Enemy>().TakeDamage(attackPower);
    }
}
