using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed; // ����ü �ӵ�
    public Vector2 direction;
    public float projectileLifeTime = 3f;
    public int damageAmount = 1;

    void Start()
    {
        Destroy(gameObject, projectileLifeTime);
    }

    void Update()
    {
        // ���⿡ �ӵ��� Time.deltaTime�� ���� ����ü �̵�
        transform.position += (Vector3)direction * speed * Time.deltaTime;

        // ����ü�� ���� �Ÿ� �̻� �־����� �ı�
        if (transform.position.magnitude > 50f)
        {
            Destroy(gameObject);
        }
    }

    // Ʈ���� �浹 ����
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // �÷��̾��� PlayerManager�� �����ͼ� ������ �ֱ�
            PlayerManager playerManager = other.GetComponent<PlayerManager>();
            if (playerManager != null)
            {
                playerManager.TakeDamage(damageAmount, "����ü ����");
            }

            Destroy(gameObject); // �浹 �� ����ü �ı�
        }

        else if(other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
