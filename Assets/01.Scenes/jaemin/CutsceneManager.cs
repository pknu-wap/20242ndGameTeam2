using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    public Image cutsceneImage; 
    public TMP_Text narrationText; 
    public CanvasGroup fadePanel; 
    public Sprite[] cutsceneImages;
    public string[] narrations; 
    public float fadeDuration = 1f;
    public float textSpeed = 0.05f;

    private int currentSceneIndex = 0; 
    private bool isSkipping = false; 

    void Start()
    {
        fadePanel.alpha = 0f;
        StartCoroutine(PlayCutscene());
    }

    void Update()
    {
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
                yield break; 
            }

            elapsed += Time.deltaTime;
            fadePanel.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            yield return null;
        }

        fadePanel.alpha = endAlpha;
    }

    IEnumerator TypeText(string text)
    {
        narrationText.text = ""; 

        foreach (char letter in text)
        {
            if (isSkipping) 
            {
                narrationText.text = text; 
                isSkipping = false;
                yield break;
            }

            narrationText.text += letter; 
            yield return new WaitForSeconds(textSpeed); 
        }
    }

    void EndCutscene()
    {
        Debug.Log("Cutscene Finished!");
        // 하늘이 튜토씬 연결
        //SceneManager.LoadScene("Tutorial");
    }
}