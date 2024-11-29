using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpUI : MonoBehaviour
{
    public GameObject levelUpPanel; // ������ UI �г�
    public Button[] optionButtons; // 3���� ��ư
    public TMP_Text[] nameTexts; // ����/���� �̸� �ؽ�Ʈ
    public TMP_Text[] infoTexts; // ����/���� ���� �ؽ�Ʈ
    public TMP_Text[] newTexts; // �ű� ���� �ؽ�Ʈ
    public Image[] optionImages; // ����/���� �̹���
    public GameManager gameManager;

    public void TriggerLevelUpUI(UpgradeOption[] options)
    {
        // 게임 멈추기
        GameManager.Instance.PauseGame();

        // UI 활성화
        levelUpPanel.SetActive(true);

        // UI 업데이트
        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (i < options.Length)
            {
                // 무기 이름 및 설명 설정
                nameTexts[i].text = options[i].name;
                infoTexts[i].text = options[i].description;
                newTexts[i].text = options[i].isNew ? "신규" : "";

                // 무기 아이콘 설정
                optionImages[i].sprite = options[i].icon;
                optionImages[i].gameObject.SetActive(true);

                // 버튼 이벤트 설정
                int index = i; // 로컬 변수 사용
                optionButtons[i].onClick.RemoveAllListeners();
                optionButtons[i].onClick.AddListener(() => SelectOption(options[index]));
                optionButtons[i].gameObject.SetActive(true);
            }
            else
            {
                // 비활성화
                optionButtons[i].gameObject.SetActive(false);
                optionImages[i].gameObject.SetActive(false);
            }
        }
    }


    public void SelectOption(UpgradeOption selectedOption)
    {
        // ������ ����/���� ��ȭ ���� ����
        UpgradeSelectedOption(selectedOption);
        // UI �г� ��Ȱ��ȭ
        CloseLevelUpUI();
    }

    private void UpgradeSelectedOption(UpgradeOption selectedOption)
    {
        if (selectedOption.name == "채찍")
            GameManager.Instance.SelectWeapon(0);
        else if (selectedOption.name == "대검")
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
        GameManager.Instance.ResumeGame(); // �Ͻ����� ����
    }

    // ����/���� �ɼ� Ŭ����
    [System.Serializable]
    public class UpgradeOption
    {
        public string name; // �̸�
        public string description; // ����
        public Sprite icon; // ������ �̹���
        public bool isNew; // �ű� ����
    }
}
