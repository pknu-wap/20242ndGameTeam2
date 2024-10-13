using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombSpawn : MonoBehaviour
{
    public GameObject bomb;
    public float spawnInterval = 2.0f;
    public enum WeaponType { MELEE, RANGED }
    public WeaponType currentWeapon = WeaponType.MELEE;

    public Button ChangeWeaponButton;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnBomb", 0f, spawnInterval);

        if (ChangeWeaponButton != null)
        {
            ChangeWeaponButton.onClick.AddListener(ChangeWeaponType);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SpawnBomb()
    {
        if (currentWeapon == WeaponType.RANGED)
        {
            Instantiate(bomb, transform.position, transform.rotation);
        }
    }

    void ChangeWeaponType()
    {
        // MELEE와 RANGED 사이에서 전환합니다.
        if (currentWeapon == WeaponType.MELEE)
        {
            currentWeapon = WeaponType.RANGED;
        }
        else
        {
            currentWeapon = WeaponType.MELEE;
        }
    }
}
