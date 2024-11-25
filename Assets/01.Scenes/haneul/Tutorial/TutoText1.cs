using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutoText1 : MonoBehaviour
{
    public TMP_Text Text1;  // TMP_Text로 변경
    public TMP_Text Text2;  // TMP_Text로 변경
    public GameObject joyrange;
    public static bool isExplain = false;
    public int TextNum = 0; // 텍스트 단계
    public Canvas PauseState;
    public GameObject Panel;
    public GameObject joystick;

    public void TutoText_Touch()
    {

        if (isExplain)
        {
            TextNum++;
            switch (TextNum)
            {
                case 1:
                    Text1.gameObject.SetActive(false);
                    Text2.gameObject.SetActive(true);
                    break;
                case 2:
                    Text2.gameObject.SetActive(false);
                    PauseState.gameObject.SetActive(false);
                    Panel.gameObject.SetActive(false);
                    joystick.gameObject.SetActive(true);
                    break;
            }
        }
    }
}
