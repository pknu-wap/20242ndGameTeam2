using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static int PlayerHp = 6;
    public GameObject[] Hp = new GameObject[PlayerHp];
    public static bool isDamage = false;
    public static bool isInvincible = false;

    private bool isBlinking = false;  // 깜빡임 효과가 실행 중인지 추적하는 변수

    private void Start()
    {
        StartCoroutine(PlayerState());
    }

    IEnumerator PlayerState()
    {
        while (true)
        {
            if (isDamage)  // 데미지 상태 확인
            {
                if (!isInvincible)  // 무적 상태가 아니면 데미지를 받음
                {
                    isInvincible = true;  // 무적 상태 시작
                    PlayerHp--;

                    if (PlayerHp >= 0 && PlayerHp < Hp.Length)
                    {
                        Hp[PlayerHp].SetActive(false);  // HP 감소 반영
                    }

                    // 깜빡임 및 무적 상태 처리
                    Invincible();

                    // 무적 상태 유지 시간
                    yield return new WaitForSeconds(2.5f);

                    isBlinking = false;  // 깜빡임 효과 종료
                    isInvincible = false;  // 무적 상태 종료
                }

                // 데미지 상태 초기화 (무조건 처리)
                isDamage = false;
            }

            yield return null;  // 다음 프레임까지 대기
        }
    }

    void Invincible()
    {
        if (!isBlinking)  // 깜빡임 효과가 실행 중이지 않으면 시작
        {
            StartCoroutine(BlinkEffect());
        }
    }

    IEnumerator BlinkEffect()
    {
        isBlinking = true;  // 깜빡임 효과가 시작됨을 표시

        Renderer renderer = GetComponent<Renderer>();
        if (renderer == null)
        {
            Debug.LogWarning("Renderer not found!");
            isBlinking = false;  // Renderer가 없으면 깜빡임 상태 리셋
            yield break;
        }

        Color originalColor = renderer.material.color;
        Color transparentColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0.5f);

        // 1.5초 동안 깜빡임 효과 수행
        float blinkInterval = 0.1f;

        for (int i = 0; i < 10; i++)
        {
            renderer.material.color = transparentColor;
            yield return new WaitForSeconds(blinkInterval);
            renderer.material.color = originalColor;
            yield return new WaitForSeconds(blinkInterval);
        }
    }
}
