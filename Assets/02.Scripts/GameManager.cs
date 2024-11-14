using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;  // 싱글톤 인스턴스
    #region 체력
    [SerializeField]
    private static int maxHealth = 6; // 최대 체력
    [SerializeField]
    private int currentHealth; // 현재 체력

    /* 쓸일 없으면 삭제.
    public int attackPower = 10; // 공격력*/
    [SerializeField]
    private GameObject[] Hp = new GameObject[maxHealth];
    public GameObject player;
    #endregion
    #region 경험치
    // 경험치, 레벨 관련 변수
    public int exp = 0; // 현재 경험치
    public int level = 1; // 현재 레벨
    public int expToNextLevel = 100; // 다음 레벨까지 필요한 경험치
    public float expMultiplier = 1.2f; // 각 레벨업마다 필요한 경험치 증가 배율
    public Slider expSlider; // 경험치 슬라이더
    public TMP_Text levelText;   // 레벨 텍스트

    // 경험치를 추가하고 레벨업을 체크하는 함수
    public void AddExperience(int amount)
    {
        exp += amount;
        Debug.Log("현재 경험치: " + exp);

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
        exp -= expToNextLevel; // 레벨업 후 경험치 초과분을 남김

        // 새로운 레벨을 위한 경험치 목표 설정 (expToNextLevel을 배율로 증가)
        expToNextLevel = Mathf.FloorToInt(expToNextLevel * expMultiplier);

        Debug.Log("레벨업! 현재 레벨: " + level + ", 다음 레벨까지 필요한 경험치: " + expToNextLevel);
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
    private int meleeWeapon1;   // 예: 1 = 검, 2 = 도끼
    [SerializeField]
    private int meleeWeapon2;   // 예: 1 = 검, 2 = 도끼
    [SerializeField]
    private int targetWeapon;   // TargetWeapon을 PlayerAttack으로 설정
    [SerializeField]
    private int nonTargetWeapon; // 비타겟 무기
    [SerializeField]
    private int maxWeaponLevel = 8; //무기 최대 레벨

    private PlayerAttack playerAttackScript; // PlayerAttack 스크립트 레퍼런스
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
        // PlayerAttack 스크립트를 초기화하고 비활성화
        /*playerAttackScript = GetComponent<PlayerAttack>();
        playerAttackScript.enabled = false;*/
        expSlider.handleRect.gameObject.SetActive(false); // 핸들 부분을 비활성화
        UpdateUI();
    }
    private void Update()
    {
        /*// TargetWeapon이 PlayerAttack인지 확인하고 활성화
        if (targetWeapon > 0 && targetWeapon <= maxLevel)
        {
            EnableWeapon(playerAttackScript);
        }
        else
        {
            DisableWeapon(playerAttackScript);
        }*/
    }

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

    // 공격 함수 (필요시 구현)
    public void Attack(GameObject target)
    {
        // 공격 시 타겟에게 피해를 입히는 코드 추가
        // 예: target.GetComponent<Enemy>().TakeDamage(attackPower);
    }

    void EnableWeapon(MonoBehaviour weaponScript)
    {
        if (!weaponScript.enabled) weaponScript.enabled = true;
    }

    void DisableWeapon(MonoBehaviour weaponScript)
    {
        if (weaponScript.enabled) weaponScript.enabled = false;
    }

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
}
