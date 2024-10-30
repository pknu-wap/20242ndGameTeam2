using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if(hitInfo.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Destroy(gameObject);
        }
        else if(hitInfo.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Destroy(gameObject);
        }
        /*if (hitInfo.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
        else if (hitInfo.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }*/
    }
}
