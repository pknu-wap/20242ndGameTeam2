using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuppetWarning : MonoBehaviour
{
    public GameObject wall;
    public GameObject Panel;
    public GameObject BossExplain;
    public Slider healthSlider;
    public Button ChangeWeapon;
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            // 부모 객체 찾기
            Transform parent = transform.parent; // 부모 객체 참조

            // 부모 객체에서 PuppetMarkMove 컴포넌트를 가져와 ChangeSprite 호출
            parent.GetComponent<PuppetMarkMove>().ChangeSprite();
            wall.SetActive(true);
            Panel.SetActive(true);
            BossExplain.SetActive(true);
            healthSlider.gameObject.SetActive(true);
            ChangeWeapon.gameObject.SetActive(true);
            // 현재 게임 오브젝트 파괴
            Destroy(gameObject);
        }
    }
}