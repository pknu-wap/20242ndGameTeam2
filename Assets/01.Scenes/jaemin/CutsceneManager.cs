using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CutsceneData
{
    public Sprite image; // 이미지
    public List<string> texts; // 텍스트 리스트
}

public class CutsceneManager : MonoBehaviour
{
    public Image cutsceneImage; // 이미지를 표시할 UI Image
    public TMP_Text narrationText; // 텍스트 (TextMeshPro)
    public CanvasGroup fadePanel; // 페이드 패널
    public List<CutsceneData> cutscenes; // 이미지와 텍스트 데이터를 관리하는 리스트
    public float fadeDuration = 1f; // 페이드 시간
    public float textSpeed = 0.05f; // 텍스트 출력 속도
    public float waitBetweenTexts = 2f; // 텍스트 출력 후 대기 시간

    private int currentSceneIndex = 0; // 현재 장면 인덱스
    private int currentTextIndex = 0; // 현재 텍스트 인덱스
    private bool isTyping = false; // 텍스트 출력 중 여부
    private bool isSkipping = false; // 스킵 여부 체크
    private bool isWaiting = false; // 대기 중 여부
    private bool isReadyForInput = false; // 입력 허용 여부

    void Start()
    {
        fadePanel.alpha = 1f; // 시작 시 페이드 아웃 상태
        StartCoroutine(PlayCutscene());
    }

    void Update()
    {
        // 클릭 입력은 페이드 인 완료 후(isReadyForInput=true)만 처리
        if (isReadyForInput && Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                isSkipping = true; // 현재 텍스트 스킵
            }
            else if (isWaiting)
            {
                isWaiting = false; // 대기 시간 건너뛰기
            }
        }
    }

    IEnumerator PlayCutscene()
    {
        while (currentSceneIndex < cutscenes.Count)
        {
            // 현재 장면 데이터
            CutsceneData currentCutscene = cutscenes[currentSceneIndex];

            // 이미지 설정 및 초기화
            cutsceneImage.sprite = currentCutscene.image;
            currentTextIndex = 0;

            // 페이드 인 (투명 → 불투명)
            isReadyForInput = false; // 입력 비활성화
            narrationText.text = ""; // 텍스트 숨김
            yield return StartCoroutine(Fade(0f, 1f, fadeDuration));
            isReadyForInput = true; // 입력 활성화

            // 텍스트 출력 반복
            while (currentTextIndex < currentCutscene.texts.Count)
            {
                yield return StartCoroutine(ShowText(currentCutscene.texts[currentTextIndex]));
                currentTextIndex++;
            }

            // 입력 비활성화 후 페이드 아웃 시작
            isReadyForInput = false; // 입력 비활성화
            narrationText.text = ""; // 텍스트 숨김
            yield return StartCoroutine(Fade(1f, 0f, fadeDuration));

            // 다음 장면으로 이동
            currentSceneIndex++;
        }

        EndCutscene();
    }

    IEnumerator ShowText(string text)
    {
        isTyping = true;
        narrationText.text = ""; // 텍스트 초기화

        // 텍스트 주르륵 출력
        foreach (char letter in text)
        {
            if (isSkipping)
            {
                narrationText.text = text; // 전체 텍스트 표시
                isSkipping = false;
                break;
            }

            narrationText.text += letter; // 한 글자씩 추가
            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false; // 텍스트 출력 완료

        // 2초 대기 또는 클릭으로 건너뛰기
        isWaiting = true;
        float elapsedTime = 0f;
        while (isWaiting && elapsedTime < waitBetweenTexts)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isWaiting = false;
    }

    IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            fadePanel.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            yield return null;
        }

        fadePanel.alpha = endAlpha;
    }

    void EndCutscene()
    {
        Debug.Log("컷씬 종료");
        // 다음 씬 전환 코드 추가 가능
        // 예: SceneManager.LoadScene("NextScene");
    }
}