using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUIManager : MonoBehaviour
{
    // Melee와 Range에 각각 4개의 슬롯
    public Image[] meleeWeaponImages; // Melee 무기 이미지 배열
    public TMP_Text[] meleeWeaponLevelTexts; // Melee 무기 레벨 텍스트 배열
    public Image[] rangeWeaponImages; // Range 무기 이미지 배열
    public TMP_Text[] rangeWeaponLevelTexts; // Range 무기 레벨 텍스트 배열

    // GameManager의 참조
    private GameManager gameManager;

    void Start()
    {
        // GameManager 참조 가져오기
        gameManager = FindObjectOfType<GameManager>();

        // UI 초기화
        UpdateWeaponUI();
    }

    void Update()
    {
        // 매 프레임마다 UI 갱신 (레벨업 시 반영)
        UpdateWeaponUI();
    }

    // 무기 UI 업데이트 함수
    private void UpdateWeaponUI()
    {
        // Melee 무기 UI 업데이트
        UpdateMeleeWeaponUI();

        // Range 무기 UI 업데이트
        UpdateRangeWeaponUI();
    }

    // Melee 무기 UI 업데이트
    private void UpdateMeleeWeaponUI()
    {
        // Melee 무기 레벨을 GameManager에서 가져와서 UI에 업데이트
        UpdateWeaponSlot(gameManager.meleeWeapon1_Level, meleeWeaponImages[0], meleeWeaponLevelTexts[0]);
        UpdateWeaponSlot(gameManager.meleeWeapon2_Level, meleeWeaponImages[1], meleeWeaponLevelTexts[1]);
        UpdateWeaponSlot(gameManager.meleeWeapon3_Level, meleeWeaponImages[2], meleeWeaponLevelTexts[2]);
        UpdateWeaponSlot(gameManager.meleeWeapon4_Level, meleeWeaponImages[3], meleeWeaponLevelTexts[3]);
    }

    // Range 무기 UI 업데이트
    private void UpdateRangeWeaponUI()
    {
        // Range 무기 레벨을 GameManager에서 가져와서 UI에 업데이트
        UpdateWeaponSlot(gameManager.longRangeAttack1_Level, rangeWeaponImages[0], rangeWeaponLevelTexts[0]);
        UpdateWeaponSlot(gameManager.longRangeAttack2_Level, rangeWeaponImages[1], rangeWeaponLevelTexts[1]);
        UpdateWeaponSlot(GameManager.Instance.longRangeAttack3_Level, rangeWeaponImages[2], rangeWeaponLevelTexts[2]);
        UpdateWeaponSlot(gameManager.longRangeAttack4_Level, rangeWeaponImages[3], rangeWeaponLevelTexts[3]);

        
    }

    // 무기 슬롯 업데이트 (레벨에 따라 UI 업데이트)
    private void UpdateWeaponSlot(int weaponLevel, Image weaponImage, TMP_Text weaponLevelText)
    {
        // 무기 레벨 텍스트 업데이트
        weaponLevelText.text = "Level: " + weaponLevel.ToString();

        // 무기 레벨이 0이면 UI 비활성화, 그 외에는 활성화
        if (weaponLevel > 0)
        {
            weaponImage.gameObject.SetActive(true);
            weaponLevelText.gameObject.SetActive(true);
        }
        else
        {
            weaponImage.gameObject.SetActive(false);
            weaponLevelText.gameObject.SetActive(false);
        }
    }
}
