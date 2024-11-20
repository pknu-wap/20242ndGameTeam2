using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Fireball : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            TutorialManager.isDamage = true;
            Destroy(gameObject);
        }
    }
}
