using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    public static bool isPaused = false;
    public Canvas pauseCanvas;   // 일시정지 버튼과 관련된 Canvas
    public GameObject ingameCanvas;  // 재개 버튼과 관련된 Canvas
    public GameObject filter;

    public void GamePause()
    {
        GameManager.Instance.PauseGame(); // 일시정지 관리
        pauseCanvas.gameObject.SetActive(true);
        ingameCanvas.gameObject.SetActive(false);
        filter.gameObject.SetActive(true);
    }

    public void GameResume()
    {
        GameManager.Instance.ResumeGame(); // 일시정지 해제
        pauseCanvas.gameObject.SetActive(false);
        ingameCanvas.gameObject.SetActive(true);
        filter.gameObject.SetActive(false);
    }
}
