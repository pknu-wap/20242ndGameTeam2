using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball_Instance : MonoBehaviour
{
    public List<GameObject> prefabs; // 프리팹 리스트
    public float spacing = 2.0f; // 프리팹 간의 간격
    public float moveSpeed = 5.0f; // 이동 속도
    public float fireRate; // 발사 주기 (초)

    private List<GameObject> instantiatedPrefabs = new List<GameObject>(); // 생성된 프리팹들

    void Start()
    {
        StartCoroutine(FirePrefabsCoroutine());
    }

    void Update()
    {
        // 생성된 프리팹들 이동
        foreach (GameObject obj in instantiatedPrefabs)
        {
            if (obj != null)
            {
                obj.transform.Translate(Vector3.left * moveSpeed * Time.deltaTime); // 항상 왼쪽으로 이동
            }
        }
    }

    IEnumerator FirePrefabsCoroutine()
    {
        while (true)
        {
            // 프리팹을 1열로 배치하여 발사
            for (int i = 0; i < prefabs.Count; i++)
            {
                Vector3 position = transform.position + new Vector3(0, i * spacing, 0); // Y축으로 배치
                GameObject instance = Instantiate(prefabs[i], position, Quaternion.identity); // 회전 없이 생성
                instantiatedPrefabs.Add(instance);
            }

            // 다음 발사를 위해 대기
            yield return new WaitForSeconds(fireRate);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject); // 자신의 파괴
        }
    }

    void OnDestroy()
    {
        // Fireball_Instance가 파괴될 때 생성된 모든 파이어볼 삭제
        foreach (GameObject fireball in instantiatedPrefabs)
        {
            if (fireball != null)
            {
                Destroy(fireball); // 생성된 파이어볼 삭제
            }
        }
    }
}



