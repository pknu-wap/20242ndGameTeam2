using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static LevelUpUI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }  // 싱글톤 인스턴스
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject arrowPoint;
    #region 체력
    [SerializeField] private static int maxHealth = 6; // 최대 체력
    [SerializeField] private int currentHealth; // 현재 체력
    [SerializeField] private GameObject[] Hp = new GameObject[maxHealth];
    [SerializeField] public static bool isInvincible = false;
    [SerializeField] public static bool isDamage= false;

    // 플레이어가 피해를 입는 함수
    public void TakeDamageToPlayer(int damage, string damageSource)
    {
        if (!isInvincible)
        {
            isDamage = true;
            
            
            currentHealth -= damage;
            Debug.Log("현재 체력 : " + Hp[currentHealth]);
            Hp[currentHealth].SetActive(false);
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // 현재 체력을 0 이하로 떨어지지 않게 하고 최대 체력보다 클 수 없게 Clamp 처리
            
            if (currentHealth <= 0)
            {
                Die(); // 체력이 0 이하가 되면 사망
            }
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
    // 근접 무기 레벨 변수
    [SerializeField] public int meleeWeapon1_Level;
    [SerializeField] public int meleeWeapon2_Level;
    [SerializeField] public int meleeWeapon3_Level;
    [SerializeField] public int meleeWeapon4_Level;

    // 원거리 무기 레벨 변수
    [SerializeField] public int longRangeAttack1_Level;
    [SerializeField] public int longRangeAttack2_Level;
    [SerializeField] public int longRangeAttack3_Level;
    [SerializeField] public int longRangeAttack4_Level;
    //무기 최대 레벨
    [SerializeField] public int maxWeaponLevel = 8;

    private LongRangeAttack1 LongRangeAttack1Script;
    private LongRangeAttack2 LongRangeAttack2Script;
    private LongRangeAttack3 LongRangeAttack3Script;
    private MeleeWeapon3 MeleeWeapon3Script;
    private MeleeWeapon4 MeleeWeapon4Script;

    public void SelectWeapon(int weaponIndex)
    {
        switch (weaponIndex)
        {
            case 0:
                if (meleeWeapon1_Level < maxWeaponLevel)
                {
                    meleeWeapon1_Level++;
                }
                break;
            case 1:
                if (meleeWeapon2_Level < maxWeaponLevel)
                {
                    meleeWeapon2_Level++;
                }
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
                if (longRangeAttack1_Level < maxWeaponLevel)
                {
                    longRangeAttack1_Level++;
                    UpdateLongRangeWeapon1Level();
                }
                break;
            case 5:
                if (longRangeAttack2_Level < maxWeaponLevel)
                {
                    longRangeAttack2_Level++;
                    UpdateLongRangeWeapon2Level();
                }
                break;
            case 6:
                if (longRangeAttack3_Level < maxWeaponLevel)
                {
                    longRangeAttack3_Level++;
                    UpdateLongRangeWeapon3Level();
                }
                break;
            case 7:
                if (longRangeAttack4_Level < maxWeaponLevel)
                {
                    longRangeAttack4_Level++;
                }
                break;
        }
        UpdateWeaponStatus();
    }

    private void UpdateWeaponStatus()
    {
        // 원거리 무기
        if (longRangeAttack1_Level == 0)
            if (LongRangeAttack1Script != null)
                LongRangeAttack1Script.enabled = false;
        else
            if (LongRangeAttack1Script != null)
                LongRangeAttack1Script.enabled = true;


        if (longRangeAttack2_Level == 0)
            if (LongRangeAttack2Script != null)
                LongRangeAttack2Script.enabled = false;
        else
            if (LongRangeAttack2Script != null)
                LongRangeAttack2Script.enabled = true;


        if (longRangeAttack3_Level == 0)
            if (LongRangeAttack3Script != null)
                LongRangeAttack3Script.enabled = false;
        else
            if (LongRangeAttack3Script != null)
                LongRangeAttack3Script.enabled = true;

        // 근접무기

        if (meleeWeapon3_Level == 0)
            if (MeleeWeapon3Script != null)
                MeleeWeapon3Script.enabled = false;
        else
            if (MeleeWeapon3Script != null)
                MeleeWeapon3Script.enabled = true;


        if (meleeWeapon4_Level == 0)
            if (MeleeWeapon4Script != null)
                MeleeWeapon4Script.enabled = false;
        else
            if (MeleeWeapon4Script != null)
                MeleeWeapon4Script.enabled = true;
    }

    public LevelUpUI.UpgradeOption[] GetUpgradeOptions()
    {
        List<UpgradeOption> options = new List<UpgradeOption>();

        // 근접 무기 1
        if (meleeWeapon1_Level < maxWeaponLevel)
        {
            string description = meleeWeapon1_Level == 0 ? "근접무기1" :
                                 meleeWeapon1_Level == 1 ? "강화된 근접무기1" :
                                 meleeWeapon1_Level == 2 ? "강화된 근접무기1" :
                                 meleeWeapon1_Level == 3 ? "강화된 근접무기1" :
                                 meleeWeapon1_Level == 4 ? "강화된 근접무기1" :
                                 meleeWeapon1_Level == 5 ? "강화된 근접무기1" :
                                 meleeWeapon1_Level == 6 ? "강화된 근접무기1" :
                                 meleeWeapon1_Level == 7 ? "강화된 근접무기1" :
                                 "강화된 대검: 추가 효과";
            options.Add(new LevelUpUI.UpgradeOption { name = "대검", description = description, icon = null, isNew = meleeWeapon1_Level == 0 });
        }

        // 근접 무기 2
        if (meleeWeapon2_Level < maxWeaponLevel)
        {
            string description = meleeWeapon2_Level == 0 ? "근접무기2" :
                                 meleeWeapon2_Level == 1 ? "강화된 근접무기2" :
                                 meleeWeapon2_Level == 2 ? "강화된 근접무기2" :
                                 meleeWeapon2_Level == 3 ? "강화된 근접무기2" :
                                 meleeWeapon2_Level == 4 ? "강화된 근접무기2" :
                                 meleeWeapon2_Level == 5 ? "강화된 근접무기2" :
                                 meleeWeapon2_Level == 6 ? "강화된 근접무기2" :
                                 meleeWeapon2_Level == 7 ? "강화된 근접무기2" :
                                 "강화된 채찍: 추가 효과";
            options.Add(new LevelUpUI.UpgradeOption { name = "채찍", description = description, icon = null, isNew = meleeWeapon2_Level == 0 });
        }

        // 근접 무기 3
        if (meleeWeapon3_Level < maxWeaponLevel)
        {
            string description = meleeWeapon3_Level == 0 ? "주변을 회전하며\n공격합니다." :
                                 meleeWeapon3_Level == 1 ? "투사체 개수 증가\n회전 속도 30% 증가" :
                                 meleeWeapon3_Level == 2 ? "투사체 개수 증가\n공격 범위 25% 증가" :
                                 meleeWeapon3_Level == 3 ? "투사체 개수 증가\n지속 시간 0.5초 증가" :
                                 meleeWeapon3_Level == 4 ? "투사체 개수 증가\n회전 속도 30% 증가" :
                                 meleeWeapon3_Level == 5 ? "투사체 개수 증가\n공격 범위 25% 증가" :
                                 meleeWeapon3_Level == 6 ? "투사체 개수 증가\n지속 시간 0.5초 증가" :
                                 meleeWeapon3_Level == 7 ? "투사체 개수 증가" :
                                 "강화된 성경: 추가 효과";
            options.Add(new LevelUpUI.UpgradeOption { name = "성경", description = description, icon = null, isNew = meleeWeapon3_Level == 0 });
        }

        // 근접 무기 4
        if (meleeWeapon4_Level < maxWeaponLevel)
        {
            string description = meleeWeapon4_Level == 0 ? "범위 내의 적에게\n피해를 줍니다." :
                                 meleeWeapon4_Level == 1 ? "공격력 1 증가 \n공격 범위 1 증가" :
                                 meleeWeapon4_Level == 2 ? "공격력 1 증가 \n공격 범위 1 증가" :
                                 meleeWeapon4_Level == 3 ? "공격력 1 증가 \n공격 범위 1 증가" :
                                 meleeWeapon4_Level == 4 ? "공격력 1 증가 \n공격 범위 1 증가" :
                                 meleeWeapon4_Level == 5 ? "공격력 1 증가 \n공격 범위 1 증가" :
                                 meleeWeapon4_Level == 6 ? "공격력 1 증가 \n공격 범위 1 증가" :
                                 meleeWeapon4_Level == 7 ? "공격력 1 증가 \n공격 범위 1 증가" :
                                 "강화된 마늘: 추가 효과";
            options.Add(new LevelUpUI.UpgradeOption { name = "마늘", description = description, icon = null, isNew = meleeWeapon4_Level == 0 });
        }

        // 원거리 무기 1
        if (longRangeAttack1_Level < maxWeaponLevel)
        {
            string description = longRangeAttack1_Level == 0 ? "가장 가까운 적을\n공격합니다." :
                                 longRangeAttack1_Level == 1 ? "투사체 개수 증가" :
                                 longRangeAttack1_Level == 2 ? "투사체 개수 증가" :
                                 longRangeAttack1_Level == 3 ? "투사체 개수 증가" :
                                 longRangeAttack1_Level == 4 ? "투사체 개수 증가" :
                                 longRangeAttack1_Level == 5 ? "쿨타임 0.25초 감소" :
                                 longRangeAttack1_Level == 6 ? "쿨타임 0.25초 감소" :
                                 longRangeAttack1_Level == 7 ? "쿨타임 0.2초 감소" :
                                 "강화된 조준경: 추가 효과";
            options.Add(new LevelUpUI.UpgradeOption { name = "조준경", description = description, icon = null, isNew = longRangeAttack1_Level == 0 });
        }

        // 원거리 무기 2
        if (longRangeAttack2_Level < maxWeaponLevel)
        {
            string description = longRangeAttack2_Level == 0 ? "캐릭터를 기준으로\n대칭을 이루며\n공격합니다." :
                                 longRangeAttack2_Level == 1 ? "투사체 개수 증가" :
                                 longRangeAttack2_Level == 2 ? "투사체 개수 증가" :
                                 longRangeAttack2_Level == 3 ? "투사체 개수 증가" :
                                 longRangeAttack2_Level == 4 ? "투사체 개수 증가" :
                                 longRangeAttack2_Level == 5 ? "투사체 개수 증가" :
                                 longRangeAttack2_Level == 6 ? "투사체 개수 증가" :
                                 longRangeAttack2_Level == 7 ? "투사체 개수 증가" :
                                 "강화된 콩알탄: 추가 효과";
            options.Add(new LevelUpUI.UpgradeOption { name = "콩알탄", description = description, icon = null, isNew = longRangeAttack2_Level == 0 });
        }

        // 원거리 무기 3
        if (longRangeAttack3_Level < maxWeaponLevel)
        {
            string description = longRangeAttack3_Level == 0 ? "바라보는 방향으로\n단검을 투척합니다." :
                                 longRangeAttack3_Level == 1 ? "투사체 개수 증가" :
                                 longRangeAttack3_Level == 2 ? "투사체 개수 증가\n공격력 5 증가" :
                                 longRangeAttack3_Level == 3 ? "투사체 개수 증가\n쿨타임 0.02초 감소" :
                                 longRangeAttack3_Level == 4 ? "관통 수 증가" :
                                 longRangeAttack3_Level == 5 ? "투사체 개수 증가\n쿨타임 0.02초 감소" :
                                 longRangeAttack3_Level == 6 ? "투사체 개수 증가\n공격력 5 증가" :
                                 longRangeAttack3_Level == 7 ? "관통 수 증가\n쿨타임 0.02초 감소" :
                                 "강화된 단검: 추가 효과";
            options.Add(new LevelUpUI.UpgradeOption { name = "단검", description = description, icon = null, isNew = longRangeAttack3_Level == 0 });
        }

        if (longRangeAttack4_Level < maxWeaponLevel)
        {
            string description = longRangeAttack4_Level == 0 ? "나선형으로 회전하며\n공격합니다." :
                                 longRangeAttack4_Level == 1 ? "투사체 개수 증가" :
                                 longRangeAttack4_Level == 2 ? "투사체 개수 증가" :
                                 longRangeAttack4_Level == 3 ? "투사체 개수 증가" :
                                 longRangeAttack4_Level == 4 ? "투사체 개수 증가" :
                                 longRangeAttack4_Level == 5 ? "쿨타임 0.25초 감소" :
                                 longRangeAttack4_Level == 6 ? "쿨타임 0.25초 감소" :
                                 longRangeAttack4_Level == 7 ? "쿨타임 0.2초 감소" :
                                 "강화된 헬리오스: 추가 효과";
            options.Add(new LevelUpUI.UpgradeOption { name = "헬리오스", description = description, icon = null, isNew = longRangeAttack4_Level == 0 });
        }
        // 유효한 옵션에서 무작위로 최대 3개 선택
        List<UpgradeOption> selectedOptions = new List<UpgradeOption>();
        int count = Mathf.Min(3, options.Count); // 최대 3개 선택

        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, options.Count);
            selectedOptions.Add(options[randomIndex]);
            options.RemoveAt(randomIndex); // 선택된 옵션 제거
        }

        if (selectedOptions.Count == 0)
        {
            levelUpPanel.SetActive(false); // LevelUpUI 비활성화
            Time.timeScale = 1f;
        }

        return selectedOptions.ToArray();
    }


    private void UpdateMeleeWeapon3Level()
    {
        MeleeWeapon3 meleeWeapon3 = player.GetComponent<MeleeWeapon3>();
        meleeWeapon3.currentLevel = meleeWeapon3_Level;
        meleeWeapon3.UpdateWeaponStats();
    }

    private void UpdateMeleeWeapon4Level()
    {
        MeleeWeapon4 meleeWeapon4 = player.GetComponent<MeleeWeapon4>();
        meleeWeapon4.currentLevel = meleeWeapon4_Level;
        meleeWeapon4.UpdateWeaponStats();
    }

    // 마법지팡이
    private void UpdateLongRangeWeapon1Level()
    {
        LongRangeAttack1 longRangeAttack1 = player.GetComponent<LongRangeAttack1>();
        longRangeAttack1.weaponLevel = longRangeAttack1_Level;
        longRangeAttack1.CalculateFirePositions();
        longRangeAttack1.UpdateAttackCooldown();
    }

    // 도끼
    private void UpdateLongRangeWeapon2Level()
    {
        LongRangeAttack2 longRangeAttack2 = player.GetComponent<LongRangeAttack2>();
        longRangeAttack2.weaponLevel = longRangeAttack2_Level;
        longRangeAttack2.UpdateWeaponStats();
    }

    // 단검
    private void UpdateLongRangeWeapon3Level()
    {
        LongRangeAttack3 longRangeAttack3 = arrowPoint.GetComponent<LongRangeAttack3>();
        longRangeAttack3.Level = longRangeAttack3_Level;  // 레벨 반영
        longRangeAttack3.UpdateStatsByLevel();  // 능력치 업데이트
    }

    #region 무기 변경
    // WeaponChange 메서드
    public void WeaponChange()
    {
        isMelee = !isMelee;  // 원거리/근거리 상태 전환

        // 상태에 맞는 무기들 활성화/비활성화
        if (isMelee)
        {
            ActivateLongRangeWeapons(false);  // 원거리 무기 비활성화
            ActivateMeleeWeapons(true);  // 근거리 무기 활성화
        }
        else
        {
            ActivateLongRangeWeapons(true);  // 원거리 무기 활성화
            ActivateMeleeWeapons(false);  // 근거리 무기 비활성화
        }
    }

    // 원거리 무기 활성화/비활성화
    private void ActivateLongRangeWeapons(bool isActive)
    {
        LongRangeAttack1Script.enabled = isActive && longRangeAttack1_Level > 0;
        LongRangeAttack2Script.enabled = isActive && longRangeAttack2_Level > 0;
    }

    // 근거리 무기 활성화/비활성화
    private void ActivateMeleeWeapons(bool isActive)
    {
        MeleeWeapon3Script.enabled = isActive && meleeWeapon3_Level > 0;
        MeleeWeapon4Script.enabled = isActive && meleeWeapon4_Level > 0;
    }
    #endregion
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
        // 원거리 무기만 활성화
        ActivateLongRangeWeapons(true);
        ActivateMeleeWeapons(false);
    }
}
