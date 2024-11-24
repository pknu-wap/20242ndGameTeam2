using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon3Bullet : MonoBehaviour
{
     public int baseDamage = 10;

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        BaseEnemy enemy = hitInfo.GetComponent<BaseEnemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(baseDamage);
        }
    }
}
