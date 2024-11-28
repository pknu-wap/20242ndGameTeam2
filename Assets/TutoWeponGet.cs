using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoWeponGet : MonoBehaviour
{
    public GameObject GetWepon;
    public ParticleSystem WeaponGetParticles;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (gameObject.name == "GetWhip")
            {
                GameManager.Instance.meleeWeapon1_Level++;
            }

            if (gameObject.name == "GetKnife")
            {
                GameManager.Instance.longRangeAttack3_Level++;
            }

            GetWepon.SetActive(true);
            WeaponGetParticles.Play();
            Destroy(gameObject);
        }
    }
}
