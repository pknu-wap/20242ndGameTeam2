using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class CutsceneManager : MonoBehaviour
{
    public Image cutsceneImage; // 컷씬 이미지
    public TMP_Text narrationText; // 텍스트 (TextMeshPro)
    public CanvasGroup fadePanel; // 페이드 패널
    public Sprite[] cutsceneImages; // 이미지 배열
    public string[] narrations; // 나래이션 배열
    public float fadeDuration = 1f; // 페이드 시간
    public float textSpeed = 0.05f; // 텍스트 출력 속도

    private int currentSceneIndex = 0; // 현재 장면 인덱스
    private bool isSkipping = false; // 스킵 여부 체크

    void Start()
    {
        fadePanel.alpha = 0f; // 패널 투명 상태로 시작
        StartCoroutine(PlayCutscene());
    }

    void Update()
    {
        // 화면 터치 시 스킵 플래그 활성화
        if (Input.GetMouseButtonDown(0))
        {
            isSkipping = true;
        }
    }

    IEnumerator PlayCutscene()
    {
        while (currentSceneIndex < cutsceneImages.Length)
        {
            // 1. 현재 이미지와 나래이션 설정
            cutsceneImage.sprite = cutsceneImages[currentSceneIndex];
            narrationText.text = "";

            // 2. 페이드 인 (투명 → 불투명)
            yield return StartCoroutine(Fade(0, 1, fadeDuration));

            // 3. 텍스트 한 글자씩 출력
            yield return StartCoroutine(TypeText(narrations[currentSceneIndex]));

            // 4. 텍스트가 모두 출력되면 잠시 대기
            yield return new WaitForSeconds(1f);

            // 5. 페이드 아웃 (불투명 → 투명)
            yield return StartCoroutine(Fade(1, 0, fadeDuration));

            // 6. 다음 장면으로 이동
            currentSceneIndex++;
        }

        EndCutscene();
    }

    IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // 터치 시 스킵
            if (isSkipping)
            {
                fadePanel.alpha = endAlpha;
                isSkipping = false;
                yield break; // 즉시 종료
            }

            elapsed += Time.deltaTime;
            fadePanel.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            yield return null;
        }

        fadePanel.alpha = endAlpha;
    }

    IEnumerator TypeText(string text)
    {
        narrationText.text = ""; // 텍스트 초기화

        foreach (char letter in text)
        {
            if (isSkipping) // 터치 시 스킵
            {
                narrationText.text = text; // 전체 텍스트 출력
                isSkipping = false;
                yield break;
            }

            narrationText.text += letter; // 한 글자씩 추가
            yield return new WaitForSeconds(textSpeed); // 글자 출력 속도 조절
        }
    }

    void EndCutscene()
    {
        Debug.Log("Cutscene Finished!");
        // 여기에 다음 씬으로 이동하는 코드 추가
    }
}