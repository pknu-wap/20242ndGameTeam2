using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private static int pauseCounter = 0; // 현재 일시정지 상태를 트래킹
    public static bool IsPaused => pauseCounter > 0;

    public static void PauseGame()
    {
        pauseCounter++;
        Time.timeScale = 0f; // 게임 정지
    }

    public static void ResumeGame()
    {
        if (pauseCounter > 0)
            pauseCounter--;

        if (pauseCounter == 0)
            Time.timeScale = 1f; // 게임 재개
    }

    public static void ResetPause()
    {
        pauseCounter = 0;
        Time.timeScale = 1f;
    }
}
