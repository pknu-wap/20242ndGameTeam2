using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float baseDamage = 10f;

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // 각 적들마다 알아서 데미지 주기
        BaseEnemy enemy = hitInfo.GetComponent<BaseEnemy>();
        if (enemy != null)
        {
            Debug.Log("한방");
            enemy.TakeDamage(baseDamage);
        }

        if(hitInfo.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Destroy(gameObject);
        }
        else if(hitInfo.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
