using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    private bool isPaused = false;

    public void GamePause()
    {
        isPaused = !isPaused;

        if (isPaused)
            Time.timeScale = 0f; // 게임 정지
        else
            Time.timeScale = 1f; // 게임 재개
    }
}
