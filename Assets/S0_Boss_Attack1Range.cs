using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S0_Boss_Attack1Range : MonoBehaviour
{
    public static bool isAttackSusses1 = false;
    public Rigidbody2D player;

    void FixedUpdate()
    {
        Collider2D playerCollider = player.GetComponent<Collider2D>();
        Collider2D RangeCollider = GetComponent<Collider2D>();

        if (playerCollider != null && RangeCollider != null)
        {
            if (RangeCollider.IsTouching(playerCollider))
            {
                isAttackSusses1 = true;
            }
            else
            {
                isAttackSusses1 = false;
            }
        }
    }
}
