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
    private bool isPaused = false;
    public GameManager gameManager;

    void Start()
    {
        levelUpPanel.SetActive(false);
    }

    public void TriggerLevelUpUI(UpgradeOption[] options)
    {
        // 인게임 정지
        Time.timeScale = 0;
        isPaused = true;

        // UI 패널 활성화
        levelUpPanel.SetActive(true);

        // 버튼 정보 업데이트
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
                optionButtons[i].onClick.AddListener(() => SelectOption(index));
            }
        }
    }

    public void SelectOption(int index)
    {
        // 선택한 무기/유물 강화 로직 실행
        UpgradeSelectedOption(index);

        // UI 패널 비활성화
        CloseLevelUpUI();
    }

    private void UpgradeSelectedOption(int index)
    {
        // 무기/유물 강화 로직 추가
        GameManager.Instance.SelectWeapon(index); // GameManager에서 무기 선택

        Debug.Log($"무기/유물 {index + 1} 강화 완료");
    }

    public void CloseLevelUpUI()
    {
        levelUpPanel.SetActive(false);
        Time.timeScale = 1; // 인게임 진행 재개
        isPaused = false;
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
