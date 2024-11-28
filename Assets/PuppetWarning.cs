using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuppetWarning : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            // 부모 객체 찾기
            Transform parent = transform.parent; // 부모 객체 참조

            // 부모 객체에서 PuppetMarkMove 컴포넌트를 가져와 ChangeSprite 호출
            parent.GetComponent<PuppetMarkMove>().ChangeSprite();

            // 현재 게임 오브젝트 파괴
            Destroy(gameObject);
        }
    }
}