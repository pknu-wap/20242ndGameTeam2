using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float baseDamage = 10f;

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        BaseEnemy enemy = hitInfo.GetComponent<BaseEnemy>();
        if (enemy != null)
        {
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
