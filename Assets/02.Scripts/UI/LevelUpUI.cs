using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpUI : MonoBehaviour
{
    public GameObject levelUpPanel; // 레벨업 UI 패널
    public Button[] optionButtons; // 3개의 버튼
    public TMP_Text[] nameTexts; // 무기/유물 이름 텍스트
    public TMP_Text[] infoTexts; // 무기/유물 정보 텍스트
    public TMP_Text[] newTexts; // 신규 여부 텍스트
    public Image[] optionImages; // 무기/유물 이미지
    public GameManager gameManager;

    public void TriggerLevelUpUI(UpgradeOption[] options)
    {
        // 게임 정지
        GameManager.Instance.PauseGame();

        // UI 패널 활성화
        levelUpPanel.SetActive(true);

        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (i < options.Length)
            {
                // 무기/유물 정보 업데이트
                nameTexts[i].text = options[i].name;
                infoTexts[i].text = options[i].description;
                newTexts[i].text = options[i].isNew ? "신규" : ""; // 신규 여부 표시
                optionImages[i].sprite = options[i].icon;

                // 버튼 클릭 이벤트 추가
                int index = i; // 로컬 변수로 캡처
                optionButtons[i].onClick.RemoveAllListeners(); // 이전 이벤트 제거
                optionButtons[i].onClick.AddListener(() => SelectOption(options[index])); // 각 버튼에 대응되는 실제 UpgradeOption 전달
            }
        }
    }

    public void SelectOption(UpgradeOption selectedOption)
    {
        // 선택한 무기/유물 강화 로직 실행
        UpgradeSelectedOption(selectedOption);
        // UI 패널 비활성화
        CloseLevelUpUI();
    }

    private void UpgradeSelectedOption(UpgradeOption selectedOption)
    {
        // 해당 무기/유물의 이름을 사용하여 실제 무기를 선택
        if (selectedOption.name == "근접무기1") // 검을 강화
        {
            GameManager.Instance.SelectWeapon(0);
        }
        else if (selectedOption.name == "근접무기2") // 도끼를 강화
        {
            GameManager.Instance.SelectWeapon(1);
        }
        else if (selectedOption.name == "근접무기3") // 타겟 무기를 강화
        {
            GameManager.Instance.SelectWeapon(2);
        }
        else if (selectedOption.name == "원거리 무기1") // 비타겟 무기를 강화
        {
            GameManager.Instance.SelectWeapon(3);
        }
        else if (selectedOption.name == "원거리 무기2") // 비타겟 무기를 강화
        {
            GameManager.Instance.SelectWeapon(4);
        }
        else if (selectedOption.name == "원거리 무기3") // 비타겟 무기를 강화
        {
            GameManager.Instance.SelectWeapon(5);
        }
    }

    public void CloseLevelUpUI()
    {
        levelUpPanel.SetActive(false);
        GameManager.Instance.ResumeGame(); // 일시정지 해제
    }

    // 무기/유물 옵션 클래스
    [System.Serializable]
    public class UpgradeOption
    {
        public string name; // 이름
        public string description; // 정보
        public Sprite icon; // 아이콘 이미지
        public bool isNew; // 신규 여부
    }
}
