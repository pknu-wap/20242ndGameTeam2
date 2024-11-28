using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOutPanel : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    public GameObject PauseState;
    public GameObject Panel;
    public float fadeDuration = 2f;  

    private void Start()
    {
        // CanvasGroup 컴포넌트를 가져옵니다.
        canvasGroup = GetComponent<CanvasGroup>();

        // CanvasGroup이 없다면 컴포넌트를 추가하거나 비활성화합니다.
        if (canvasGroup == null)
        {
            Debug.LogWarning("CanvasGroup 컴포넌트가 이 오브젝트에 없습니다!");
            return;
        }

        // 초기 투명도 설정 (처음에 불투명하게 설정)
        canvasGroup.alpha = 1f;

        // 투명도 감소 후 사라지게 설정
        StartCoroutine(FadeOutCoroutine());
    }

    private IEnumerator FadeOutCoroutine()
    {
        float timeElapsed = 0f;

        // 투명도 점진적으로 감소
        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, timeElapsed / fadeDuration);  // 점진적으로 투명도 감소
            yield return null;
        }

        // 최종적으로 완전히 사라짐
        canvasGroup.alpha = 0f;

        Panel.gameObject.SetActive(true);

        PauseState.gameObject.SetActive(true);
        TutoText1.isExplain = true;
        
        gameObject.SetActive(false);
    }
}