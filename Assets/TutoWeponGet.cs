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
            GetWepon.SetActive(true);
            WeaponGetParticles.Play();
            Destroy(gameObject);
        }
    }
}
