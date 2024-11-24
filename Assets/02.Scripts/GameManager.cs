using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }  // 싱글톤 인스턴스
    [SerializeField] private GameObject player;
    #region 체력
    [SerializeField] private static int maxHealth = 6; // 최대 체력
    [SerializeField] private int currentHealth; // 현재 체력
    [SerializeField] private GameObject[] Hp = new GameObject[maxHealth];

    // 플레이어가 피해를 입는 함수
    public void TakeDamageToPlayer(int damage, string damageSource)
    {
        currentHealth -= damage;
        Debug.Log("현재 체력 : " + Hp[currentHealth]);
        Hp[currentHealth].SetActive(false);
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // 현재 체력을 0 이하로 떨어지지 않게 하고 최대 체력보다 클 수 없게 Clamp 처리

        if (currentHealth <= 0)
        {
            Die(); // 체력이 0 이하가 되면 사망
        }
    }

    // 사망 처리 함수
    private void Die()
    {
        player.SetActive(false);
        // 사망 시 처리할 코드 추가 (예: 게임 오버 화면 표시 등)
    }
    #endregion
    #region 경험치, 레벨업
    // 경험치, 레벨 관련 변수
    [SerializeField] private int exp = 0; // 현재 경험치
    [SerializeField] private int level = 1; // 현재 레벨
    [SerializeField] private int expToNextLevel = 100; // 다음 레벨까지 필요한 경험치
    [SerializeField] private float expMultiplier = 1.2f; // 각 레벨업마다 필요한 경험치 증가 배율
    [SerializeField] private Slider expSlider; // 경험치 슬라이더
    [SerializeField] private TMP_Text levelText;   // 레벨 텍스트
    [SerializeField] private LevelUpUI levelUpUI;  // LevelUpUI 스크립트
    [SerializeField] private GameObject levelUpPanel;

    // 
    public void AddExperience(int amount)
    {
        exp += amount;
        // 경험치가 필요한 양을 넘으면 레벨업
        if (exp >= expToNextLevel)
        {
            LevelUp();
        }
        UpdateUI();
    }

    // 레벨업 처리
    private void LevelUp()
    {
        level++;
        exp = 0;
        expToNextLevel = Mathf.FloorToInt(expToNextLevel * expMultiplier);
        DisplayLevelUpUI();
    }

    // 레벨업 UI 활성화
    private void DisplayLevelUpUI()
    {
        LevelUpUI.UpgradeOption[] options = GetUpgradeOptions();
        levelUpUI.TriggerLevelUpUI(options); // 레벨업 UI를 표시하는 함수 호출
    }


    // 레벨업 UI 업데이트 함수
    private void UpdateUI()
    {
        // 슬라이더와 레벨 텍스트 업데이트
        expSlider.maxValue = expToNextLevel;
        expSlider.value = exp;
        levelText.text = "Level: " + level;
    }
    #endregion
    #region 무기
    public static bool isMelee = false;
    [SerializeField] public int meleeWeapon1_Level;
    [SerializeField] public int meleeWeapon2_Level;
    [SerializeField] public int meleeWeapon3_Level;
    [SerializeField] public int meleeWeapon4_Level;
    [SerializeField] public int longRangeAttack1_Level;   // TargetWeapon을 PlayerAttack으로 설정
    [SerializeField] public int longRangeAttack2_Level;
    [SerializeField] public static int longRangeAttack3_Level;
    [SerializeField] public int maxWeaponLevel = 8; //무기 최대 레벨

    private LongRangeAttack1 LongRangeAttack1Script;
    private LongRangeAttack2 LongRangeAttack2Script;
    private LongRangeAttack3 LongRangeAttack3Script;
    private MeleeWeapon3 MeleeWeapon3Script;
    private MeleeWeapon4 MeleeWeapon4Script;

    public void WeaponChange()
    {
        if (isMelee == true)
        {
            isMelee = false;
        }
        else //원거리공격일때 근접공격으로 전환
        {
            isMelee = true;
        }
    }

    public void SelectWeapon(int weaponIndex)
    {
        switch (weaponIndex)
        {
            case 0:
                if (meleeWeapon1_Level < maxWeaponLevel) meleeWeapon1_Level++;
                break;
            case 1:
                if (meleeWeapon2_Level < maxWeaponLevel) meleeWeapon2_Level++;
                break;
            case 2:
                if (meleeWeapon3_Level < maxWeaponLevel) 
                {
                    meleeWeapon3_Level++;
                    UpdateMeleeWeapon3Level();
                }
                break;
            case 3:
                if (meleeWeapon4_Level < maxWeaponLevel)
                {
                    meleeWeapon4_Level++;
                    UpdateMeleeWeapon4Level();
                }
                break;
            case 4:
                if (longRangeAttack1_Level < maxWeaponLevel) longRangeAttack1_Level++;
                break;
            case 5:
                if (longRangeAttack2_Level < maxWeaponLevel) longRangeAttack2_Level++;
                break;
            case 6:
                if (longRangeAttack3_Level < maxWeaponLevel) longRangeAttack3_Level++;
                break;
        }

        UpdateWeaponStatus();
    }

    private void UpdateWeaponStatus()
    {
        if (longRangeAttack1_Level == 0)
        {
            if (LongRangeAttack1Script != null)
            {
                LongRangeAttack1Script.enabled = false;
            }
        }
        else
        {
            if (LongRangeAttack1Script != null)
            {
                LongRangeAttack1Script.enabled = true;
            }
        }

        if (longRangeAttack2_Level == 0)
        {
            if (LongRangeAttack2Script != null)
            {
                LongRangeAttack2Script.enabled = false;
            }
        }
        else
        {
            if (LongRangeAttack2Script != null)
            {
                LongRangeAttack2Script.enabled = true;
            }
        }

        if (longRangeAttack3_Level == 0)
        {
            if (LongRangeAttack3Script != null)
            {
                LongRangeAttack3Script.enabled = false;
            }
        }
        else
        {
            if (LongRangeAttack3Script != null)
            {
                LongRangeAttack3Script.enabled = true;
            }
        }

        if (meleeWeapon3_Level == 0)
        {
            if (MeleeWeapon3Script != null)
            {
                MeleeWeapon3Script.enabled = false;
            }
        }
        else
        {
            if (MeleeWeapon3Script != null)
            {
                MeleeWeapon3Script.enabled = true;
            }
        }

        if (meleeWeapon4_Level == 0)
        {
            if (MeleeWeapon4Script != null)
            {
                MeleeWeapon4Script.enabled = false;
            }
        }
        else
        {
            if (MeleeWeapon4Script != null)
            {
                MeleeWeapon4Script.enabled = true;
            }
        }
    }

    private LevelUpUI.UpgradeOption[] GetUpgradeOptions()
    {
        List<LevelUpUI.UpgradeOption> options = new List<LevelUpUI.UpgradeOption>();

        options.Add(new LevelUpUI.UpgradeOption { name = "근접무기1", description = "강화된 근접무기1", icon = null, isNew = meleeWeapon1_Level == 0 });
        options.Add(new LevelUpUI.UpgradeOption { name = "근접무기2", description = "강화된 근접무기2", icon = null, isNew = meleeWeapon2_Level == 0 });
        options.Add(new LevelUpUI.UpgradeOption { name = "근접무기3", description = "강화된 근접무기3", icon = null, isNew = meleeWeapon3_Level == 0 });
        options.Add(new LevelUpUI.UpgradeOption { name = "근접무기4", description = "강화된 근접무기4", icon = null, isNew = meleeWeapon4_Level == 0 });
        options.Add(new LevelUpUI.UpgradeOption { name = "원거리 무기1", description = "강화된 원거리 무기1", icon = null, isNew = longRangeAttack1_Level == 0 });
        options.Add(new LevelUpUI.UpgradeOption { name = "원거리 무기2", description = "강화된 원거리 무기2", icon = null, isNew = longRangeAttack2_Level == 0 });
        options.Add(new LevelUpUI.UpgradeOption { name = "원거리 무기3", description = "강화된 원거리 무기3", icon = null, isNew = longRangeAttack3_Level == 0 });

        // 무기 옵션 중 3개를 랜덤으로 선택
        List<LevelUpUI.UpgradeOption> selectedOptions = new List<LevelUpUI.UpgradeOption>();
        for (int i = 0; i < 3; i++)
        {
            int randomIndex = Random.Range(0, options.Count);
            selectedOptions.Add(options[randomIndex]);
            options.RemoveAt(randomIndex);  // 이미 선택된 옵션은 리스트에서 제거
        }

        return selectedOptions.ToArray();
    }
    private void UpdateMeleeWeapon3Level()
    {
        // Player 객체의 MeleeWeapon3 스크립트 참조
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            MeleeWeapon3 meleeWeapon3 = player.GetComponent<MeleeWeapon3>();
            if (meleeWeapon3 != null)
            {
                meleeWeapon3.currentLevel = meleeWeapon3_Level;  // 레벨 반영
                meleeWeapon3.UpdateWeaponStats();  // 능력치 업데이트
            }
        }
    }

    private void UpdateMeleeWeapon4Level()
    {
        // Player 객체의 MeleeWeapon3 스크립트 참조
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            MeleeWeapon4 meleeWeapon4 = player.GetComponent<MeleeWeapon4>();
            if (meleeWeapon4 != null)
            {
                meleeWeapon4.currentLevel = meleeWeapon4_Level;  // 레벨 반영
                meleeWeapon4.UpdateWeaponStats();  // 능력치 업데이트
            }
        }
    }
    #endregion
    #region Pause
    [SerializeField] private int pauseCounter = 0;

    public void PauseGame()
    {
        if (levelUpPanel.activeSelf == false)
        {
            if (pauseCounter == 0)
            {
                pauseCounter++; // pauseCounter 증가
                Time.timeScale = 0f; // 게임 멈춤
            }
        }
    }

    public void ResumeGame()
    {
        // 레벨업 UI가 활성화되지 않았다면 게임을 재개
        if (levelUpPanel.activeSelf == false && pauseCounter > 0)
        {
            pauseCounter--; // pauseCounter 감소
            if (pauseCounter == 0)
            {
                Time.timeScale = 1f; // 게임 재개
            }
        }
    }
    #endregion

    private void Awake()
    {
        // 싱글톤 인스턴스를 설정합니다.
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // 인스턴스가 중복될 경우 삭제
            return;
        }
        DontDestroyOnLoad(gameObject); // 씬이 변경되더라도 인스턴스 유지
    }

    void Start()
    {
        currentHealth = maxHealth; // 현재 체력을 최대 체력으로 초기화
        expSlider.handleRect.gameObject.SetActive(false); // 핸들 부분을 비활성화
        UpdateUI();
        DisplayLevelUpUI();
        // PlayerAttack 스크립트 찾기 (Player 객체에 부착되어 있다고 가정)
        LongRangeAttack1Script = GameObject.FindWithTag("Player").GetComponent<LongRangeAttack1>();
        LongRangeAttack2Script = GameObject.FindWithTag("Player").GetComponent<LongRangeAttack2>();
        LongRangeAttack3Script = GameObject.FindWithTag("Player").GetComponent<LongRangeAttack3>();
        MeleeWeapon3Script = GameObject.FindWithTag("Player").GetComponent<MeleeWeapon3>();
        MeleeWeapon4Script = GameObject.FindWithTag("Player").GetComponent<MeleeWeapon4>();
        // 무기 상태에 맞게 PlayerAttack 스크립트 활성화/비활성화
        UpdateWeaponStatus();
    }

    // 공격 함수 (필요시 구현)
    public void Attack(GameObject target)
    {
        // 공격 시 타겟에게 피해를 입히는 코드 추가
        // 예: target.GetComponent<Enemy>().TakeDamage(attackPower);
    }
}
