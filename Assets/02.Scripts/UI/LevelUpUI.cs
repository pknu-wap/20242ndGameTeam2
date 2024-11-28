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

        // 유효한 옵션 생성 및 랜덤 선택
        UpgradeOption[] validOptions = GameManager.Instance.GetUpgradeOptions();

        // UI 업데이트
        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (i < validOptions.Length)
            {
                // 무기/유물 정보 업데이트
                nameTexts[i].text = validOptions[i].name;
                infoTexts[i].text = validOptions[i].description;
                newTexts[i].text = validOptions[i].isNew ? "신규" : ""; // 신규 여부 표시
                optionImages[i].sprite = validOptions[i].icon;

                // 버튼 클릭 이벤트 추가
                int index = i; // 로컬 변수로 캡처
                optionButtons[i].onClick.RemoveAllListeners(); // 이전 이벤트 제거
                optionButtons[i].onClick.AddListener(() => SelectOption(validOptions[index])); // 옵션 선택 처리
            }
            else
            {
                // 나머지 버튼/텍스트 숨기기
                optionButtons[i].gameObject.SetActive(false);
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
        if (selectedOption.name == "대검")
            GameManager.Instance.SelectWeapon(0);
        else if (selectedOption.name == "채찍")
            GameManager.Instance.SelectWeapon(1);
        else if (selectedOption.name == "성경")
            GameManager.Instance.SelectWeapon(2);
        else if (selectedOption.name == "마늘")
            GameManager.Instance.SelectWeapon(3);
        else if (selectedOption.name == "조준경")
            GameManager.Instance.SelectWeapon(4);
        else if (selectedOption.name == "콩알탄")
            GameManager.Instance.SelectWeapon(5);
        else if (selectedOption.name == "단검")
            GameManager.Instance.SelectWeapon(6);
        else if (selectedOption.name == "헬리오스")
            GameManager.Instance.SelectWeapon(7);
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
