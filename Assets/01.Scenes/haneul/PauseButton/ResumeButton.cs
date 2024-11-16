using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResumeButton : MonoBehaviour
{
    public Canvas pauseCanvas;   // 일시정지 버튼과 관련된 Canvas
    public GameObject ingameCanvas;  // 재개 버튼과 관련된 Canvas
    public GameObject filter;

    public void GameResume()
    {
        PauseButton.isPaused = false;
        
        if (PauseButton.isPaused == false)
        {
            pauseCanvas.gameObject.SetActive(false);
            ingameCanvas.gameObject.SetActive(true);
            filter.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
