using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameStartPanel : MonoBehaviour
{
    public CanvasGroup canvasGroup; // 페이드 아웃할 대상

    private void Awake()
    {
        // Invoke를 사용할 때 메서드 이름을 문자열로 전달해야 합니다.
        Invoke(nameof(StartFadeOut), 2f);
    }

    private void StartFadeOut()
    {
        // 페이드 아웃을 시작하는 함수 호출
        FadeOutUI(1f); // 1초 동안 페이드 아웃
    }

    public void FadeOutUI(float duration)
    {
        // CanvasGroup의 alpha 값을 0으로 애니메이션
        canvasGroup.DOFade(0f, duration).OnComplete(() =>
        {
            // 페이드 아웃 완료 후 오브젝트 비활성화
            canvasGroup.gameObject.SetActive(false);
        });
    }
}
