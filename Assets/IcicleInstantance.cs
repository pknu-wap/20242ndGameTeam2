using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcicleInstantance : MonoBehaviour
{
    public GameObject Icicle;

    void Awake()
    {
        StartCoroutine(IcicleInstant());
    }

    private IEnumerator IcicleInstant()
    {
        yield return new WaitForSeconds(0.3f);

        // 월드 좌표에서 스폰 위치 가져오기
        Vector3 spawnposition = gameObject.transform.position + new Vector3(0, 0.5f, 0); // Vector3로 위치 추가

        // Icicle을 해당 위치에 스폰
        GameObject instantIcicle = Instantiate(Icicle, spawnposition, Quaternion.Euler(0, 0, 90));
        yield return new WaitForSeconds(0.4f);
        Destroy(gameObject);
    }
}
