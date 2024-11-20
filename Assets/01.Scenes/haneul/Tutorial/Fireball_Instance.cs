using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball_Instance : MonoBehaviour
{
    public List<GameObject> prefabs; // 프리팹 리스트
    public float spacing = 2.0f; // 프리팹 간의 간격
    public Vector3 moveDirection = Vector3.left; // 이동 방향 (왼쪽)
    public float moveSpeed = 5.0f; // 이동 속도

    private List<GameObject> instantiatedPrefabs = new List<GameObject>(); // 생성된 프리팹들

    void Start()
    {
        // 프리팹을 1열로 배치
        for (int i = 0; i < prefabs.Count; i++)
        {
            Vector3 position = transform.position + new Vector3(0, i * spacing, 0); // Y축으로 배치
            Quaternion rotation = Quaternion.Euler(0, 180, 0); // 프리팹을 Y축 기준으로 반대로 회전
            GameObject instance = Instantiate(prefabs[i], position, rotation);
            instantiatedPrefabs.Add(instance);
        }
    }

    void Update()
    {
        // 생성된 프리팹들 이동
        foreach (GameObject obj in instantiatedPrefabs)
        {
            if (obj != null)
            {
                obj.transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
            }
        }
    }
}
