using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Fireball : MonoBehaviour
{
    public int damage = 1;
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player") && !GameManager.isInvincible)
        {
            TakeDamageToPlayer();
            Destroy(gameObject);
        }
    }

    private void TakeDamageToPlayer()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.TakeDamageToPlayer(damage, "파이어볼");
        }
    }
}
