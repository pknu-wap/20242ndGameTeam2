using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public int PlayerHp = 6;
    public GameObject[] Hp;
    public static bool isDamage = false;
    public bool isInvincible = false;

    void Start()
    {
        Hp = new GameObject[PlayerHp];
        StartCoroutine(PlayerState());
    }

    IEnumerator PlayerState()
    {
        while (!isInvincible)
        {
            if(isDamage)
            {
                PlayerHp--;
                Hp[PlayerHp].SetActive(false);
                isDamage = false;
                isInvincible = true;

                yield return new WaitForSeconds(0.5f);
                isInvincible = false;
            }
        }
    }

    void Invincible()
    {
        StartCoroutine(BlinkEffect());
    }

    IEnumerator BlinkEffect()
    {
        Renderer renderer = GetComponent<Renderer>();
        Color originalColor = renderer.material.color;
        Color transparentColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);

        for (int i = 0; i < 5; i++) // 깜빡이는 횟수: 5번
        {
            renderer.material.color = transparentColor;
            yield return new WaitForSeconds(0.1f); // 투명 상태 유지 시간
            renderer.material.color = originalColor;
            yield return new WaitForSeconds(0.1f); // 원래 상태 유지 시간
        }
    }

}
