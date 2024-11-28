using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutoText1 : MonoBehaviour
{
    public TMP_Text Text1;  // TMP_Text로 변경
    public TMP_Text Text2;  // TMP_Text로 변경
    public TMP_Text Text3;  // TMP_Text로 변경
    public TMP_Text Text4;  // TMP_Text로 변경
    public TMP_Text Text5;  // TMP_Text로 변경
    public GameObject joyrange;
    public GameObject dodgeButton;
    public static bool isExplain = false;
    public int TextNum = 1; // 텍스트 단계
    public GameObject PauseState;
    public GameObject Panel;
    public GameObject joystick;
    public GameObject fireInstance;
    public GameObject fireInstance2;

    public void TutoText_Touch()
    {

        if (isExplain)
        {

            switch (TextNum++)
            {
                case 1:
                    Text1.gameObject.SetActive(false);
                    Text2.gameObject.SetActive(true);
                    joyrange.gameObject.SetActive(false);
                    break;
                case 2:
                    Text2.gameObject.SetActive(false);
                    PauseState.gameObject.SetActive(false);
                    joystick.gameObject.SetActive(true);
                    Panel.gameObject.SetActive(false);
                    joystick.gameObject.SetActive(true);
                    Text3.gameObject.SetActive(true);
                    break;
                case 3:
                    Text3.gameObject.SetActive(false);
                    Text4.gameObject.SetActive(true);
                    dodgeButton.gameObject.SetActive(true);
                    break;
                case 4:
                    fireInstance.SetActive(true);
                    fireInstance2.SetActive(true);
                    Text4.gameObject.SetActive(false);
                    PauseState.gameObject.SetActive(false);
                    Text5.gameObject.SetActive(true);

                    break;
                case 5:
                    Text5.gameObject.SetActive(false);
                    PauseState.gameObject.SetActive(false);

                    break;
            }
        }
    }
}
