using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeleeWeapon4 : MonoBehaviour // 마늘
{
    public GameObject meleeWeapon4Prefab; // 프리팹
    [SerializeField] public int currentLevel = 0; // 무기 레벨
    [SerializeField] private float baseDamage = 10f; // 기본 공격력
    [SerializeField] public float damageCooldown = 1f; // 공격 쿨타임 (초)
    [SerializeField] private float attackRange = 8f; // 공격 범위 (레벨에 따라 변경)
    private Transform playerTransform; // 플레이어의 Transform
    private GameObject prefabInstance; // 생성된 프리팹 인스턴스

    private Dictionary<BaseEnemy, float> enemyCooldowns = new Dictionary<BaseEnemy, float>(); // 적의 쿨타임 관리

    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        UpdateWeaponStats();
        prefabInstance = Instantiate(meleeWeapon4Prefab, playerTransform.position, Quaternion.identity, playerTransform);
        prefabInstance.transform.localScale = new Vector3(attackRange, attackRange, 1f); // 초기 크기 설정
    }

    // 무기 레벨에 따른 상태 업데이트
    public void UpdateWeaponStats()
    {
        // 레벨에 따른 공격 범위 및 프리팹 크기 업데이트
        switch (currentLevel)
        {
            case 1:
                attackRange = 8f;
                baseDamage++;
                break;
            case 2:
                attackRange = 9f;
                baseDamage++;
                break;
            case 3:
                attackRange = 10f;
                baseDamage++;
                break;
            case 4:
                attackRange = 11f;
                baseDamage++;
                break;
            case 5:
                attackRange = 12f;
                baseDamage++;
                break;
            case 6:
                attackRange = 13f;
                baseDamage++;
                break;
            case 7:
                attackRange = 14f;
                baseDamage++;
                break;
            case 8:
                attackRange = 15f;
                baseDamage++;
                break;
                // 추가 레벨에 따른 처리
        }

        // 프리팹 크기 업데이트
        if (prefabInstance != null)
        {
            prefabInstance.transform.localScale = new Vector3(attackRange, attackRange, 1f);
        }
    }

    // 적이 범위에 들어왔을 때 데미지 주는 메서드
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // BaseEnemy 컴포넌트를 가진 적이 범위 내에 들어왔을 때
        BaseEnemy enemy = hitInfo.GetComponent<BaseEnemy>();
        if (enemy != null)
        {
            if (!enemyCooldowns.ContainsKey(enemy))
            {
                // 적이 처음 들어온 경우, 쿨타임을 설정하고 데미지를 즉시 주기
                enemyCooldowns.Add(enemy, Time.time);
                enemy.TakeDamage(baseDamage); // 데미지 주기
            }
        }
    }

    void Update()
    {
        // 모든 적들에 대해 쿨타임을 갱신하고, 쿨타임이 지난 적에게 다시 데미지를 주기
        List<BaseEnemy> enemiesToDamage = new List<BaseEnemy>();
        List<BaseEnemy> enemiesToRemove = new List<BaseEnemy>();

        foreach (var entry in enemyCooldowns)
        {
            BaseEnemy enemy = entry.Key;

            // 적이 null이거나 Destroy되었는지 확인
            if (enemy == null)
            {
                enemiesToRemove.Add(enemy); // 제거할 적 목록에 추가
                continue;
            }

            float lastDamageTime = entry.Value;

            // 쿨타임이 지난 경우, 데미지를 다시 주기
            if (Time.time - lastDamageTime >= damageCooldown)
            {
                enemiesToDamage.Add(enemy);
            }
        }

        // 쿨타임이 지난 적들에게 데미지 주기
        foreach (BaseEnemy enemy in enemiesToDamage)
        {
            if (enemy != null) // 적이 존재하는 경우만 데미지 주기
            {
                enemy.TakeDamage(baseDamage); // 데미지 주기
                enemyCooldowns[enemy] = Time.time; // 마지막 데미지 시간을 갱신
            }
        }

        // 삭제된 적을 딕셔너리에서 제거
        foreach (BaseEnemy enemy in enemiesToRemove)
        {
            enemyCooldowns.Remove(enemy);
        }

        // 레벨에 따라 프리팹 크기 업데이트
        UpdateWeaponStats();
    }
}
