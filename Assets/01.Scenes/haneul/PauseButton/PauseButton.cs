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
        isPaused = true;

        if (isPaused)
        {
            pauseCanvas.gameObject.SetActive(true);
            ingameCanvas.gameObject.SetActive(false);
            filter.gameObject.SetActive(true);
            Time.timeScale = 0f; // 게임 재개
        }
            
    }
}
