using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutoWeaponChange : MonoBehaviour
{
    public GameObject PauseState;
    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
            PauseState.SetActive(true);

            Destroy(gameObject);
        }
    }
}
