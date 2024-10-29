using UnityEngine;

public class Bullet : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
        else if (hitInfo.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}