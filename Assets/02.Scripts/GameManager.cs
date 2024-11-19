using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }  // 싱글톤 인스턴스
    public GameObject player;
    #region 체력
    [SerializeField]
    private static int maxHealth = 6; // 최대 체력
    [SerializeField]
    private int currentHealth; // 현재 체력

    [SerializeField]
    private GameObject[] Hp = new GameObject[maxHealth];

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
    public int exp = 0; // 현재 경험치
    public int level = 1; // 현재 레벨
    public int expToNextLevel = 100; // 다음 레벨까지 필요한 경험치
    public float expMultiplier = 1.2f; // 각 레벨업마다 필요한 경험치 증가 배율
    public Slider expSlider; // 경험치 슬라이더
    public TMP_Text levelText;   // 레벨 텍스트
    public LevelUpUI levelUpUI;  // LevelUpUI 스크립트

    // 경험치를 추가하고 레벨업을 체크하는 함수
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
        exp -= expToNextLevel;
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
    [SerializeField]
    public int meleeWeapon1;
    [SerializeField]
    public int meleeWeapon2;
    [SerializeField]
    public int meleeWeapon3;
    [SerializeField]
    public int longRangeAttack1;   // TargetWeapon을 PlayerAttack으로 설정
    [SerializeField]
    public int longRangeAttack2;
    [SerializeField]
    public int longRangeAttack3;
    [SerializeField]
    private int maxWeaponLevel = 8; //무기 최대 레벨

    private LongRangeAttack1 LongRangeAttack1Script;
    private LongRangeAttack2 LongRangeAttack2Script;
    private LongRangeAttack3 LongRangeAttack3Script;
    private MeleeWeapon3 MeleeWeapon3Script;

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
            case 0: // meleeWeapon1 선택
                if (meleeWeapon1 < maxWeaponLevel) meleeWeapon1++;
                break;
            case 1: // meleeWeapon2 선택
                if (meleeWeapon2 < maxWeaponLevel) meleeWeapon2++;
                break;
            case 2: // meleeWeapon3 선택
                if (meleeWeapon3 < maxWeaponLevel) meleeWeapon3++;
                break;
            case 3: // longRangeAttack1 선택
                if (longRangeAttack1 < maxWeaponLevel) longRangeAttack1++;
                break;
            case 4: // longRangeAttack2 선택
                if (longRangeAttack2 < maxWeaponLevel) longRangeAttack2++;
                break;
            case 5: // longRangeAttack3 선택
                if (longRangeAttack3 < maxWeaponLevel) longRangeAttack3++;
                break;
        }

        UpdateWeaponStatus();
    }

    private void UpdateWeaponStatus()
    {
        // 원거리 무기1 (타겟팅)
        if (longRangeAttack1 == 0)
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

        // 원거리 무기2 (여러 방향으로 발사)
        if (longRangeAttack2 == 0)
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

        // 원거리 무기3 (플레이어가 보는 방향으로 발사)
        if (longRangeAttack3 == 0)
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

        // 근접 무기3 (성경)
        if (meleeWeapon3 == 0)
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
    }

    private LevelUpUI.UpgradeOption[] GetUpgradeOptions()
    {
        List<LevelUpUI.UpgradeOption> options = new List<LevelUpUI.UpgradeOption>();

        options.Add(new LevelUpUI.UpgradeOption { name = "근접무기1", description = "강화된 근접무기1", icon = null, isNew = meleeWeapon1 == 0 });
        options.Add(new LevelUpUI.UpgradeOption { name = "근접무기2", description = "강화된 근접무기2", icon = null, isNew = meleeWeapon2 == 0 });
        options.Add(new LevelUpUI.UpgradeOption { name = "근접무기3", description = "강화된 근접무기3", icon = null, isNew = meleeWeapon3 == 0 });
        options.Add(new LevelUpUI.UpgradeOption { name = "원거리 무기1", description = "강화된 원거리 무기1", icon = null, isNew = longRangeAttack1 == 0 });
        options.Add(new LevelUpUI.UpgradeOption { name = "원거리 무기2", description = "강화된 원거리 무기2", icon = null, isNew = longRangeAttack2 == 0 });
        options.Add(new LevelUpUI.UpgradeOption { name = "원거리 무기3", description = "강화된 원거리 무기3", icon = null, isNew = longRangeAttack3 == 0 });

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
    #endregion
    #region Pause
    [SerializeField]
    private int pauseCounter = 0;

    public void PauseGame()
    {
        if (pauseCounter == 0)
        {
            Time.timeScale = 0f; // 게임 멈춤
        }
        pauseCounter++;
    }

    public void ResumeGame()
    {
        if (pauseCounter > 0)
        {
            pauseCounter--;

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
        UpdateWeaponStatus();
    }

    // 공격 함수 (필요시 구현)
    public void Attack(GameObject target)
    {
        // 공격 시 타겟에게 피해를 입히는 코드 추가
        // 예: target.GetComponent<Enemy>().TakeDamage(attackPower);
    }
}
