using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage0_Boss_Judgment : MonoBehaviour
{
    public static bool isSkillSusses = false;
    public Rigidbody2D player;

    void FixedUpdate()
    {
        Collider2D playerCollider = player.GetComponent<Collider2D>();
        Collider2D RangeCollider = GetComponent<Collider2D>();

        if (playerCollider != null && RangeCollider != null)
        {
            if (RangeCollider.IsTouching(playerCollider))
            {
                isSkillSusses = true;
            }
            else
            {
                isSkillSusses = false;
            }
        }
    }
}

