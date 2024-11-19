using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon3Bullet : MonoBehaviour
{
    public float baseDamage = 10f;

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // 각 적들마다 알아서 데미지 주기
        BaseEnemy enemy = hitInfo.GetComponent<BaseEnemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(baseDamage);
        }
    }
}
