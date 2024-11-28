using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutoPotal : MonoBehaviour
{
    public GameObject WeaponGet1;
    public GameObject WeaponGet2;
    void Awake()
    {
        WeaponGet1.SetActive(false);
        WeaponGet2.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {
        
            SceneManager.LoadScene("Stage0");
        }
    }
}
