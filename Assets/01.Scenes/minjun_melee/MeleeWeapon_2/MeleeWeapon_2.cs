using System.Collections;
using UnityEngine;

public class MeleeWeapon_2 : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] private int weaponLevel = 1; // 무기 레벨
    [SerializeField] private float attackCooldown = 0.5f; // 공격 쿨타임
    [SerializeField] private float damage = 0;

    [Header("Skill Parameters")]
    [SerializeField] private float skillRadius = 5f; // 스킬 최대 거리
    [SerializeField] private float skillAngle = 60f; // 부채꼴 각도
    [SerializeField] private float timerDuration = 5f; // 타이머 지속 시간

    [Header("Effect Settings")]
    [SerializeField] private Material timerMaterial; // 붉은색 타이머 머티리얼
    [SerializeField] private LayerMask enemyLayer; // 적 레이어 마스크
    [SerializeField] private GameObject coneEffectPrefab; // 파란색 부채꼴 스프라이트 프리팹
    [SerializeField] private Transform swordObject; // 검 오브젝트

    [Header("Debug")]
    public Joystick joystick; // 조이스틱 입력
    private Vector2 moveDirection; // 입력 방향

    private GameObject blueCone; // 파란색 부채꼴
    private float fixedStartAngle; // 고정된 시작 각도
    private float fixedEndAngle; // 고정된 끝 각도
    private bool isSkillActive = false; // 스킬 활성화 상태
    private float lastAttackTime; // 마지막 공격 시간

    float scaleXY = 0f;
    float scaleSwordXY = 0;

    void Start()
    {
        // 초기화 로직
        timerMaterial.SetFloat("_StartAngle", 0);
        timerMaterial.SetFloat("_FillAngle", 0);

        blueCone = Instantiate(coneEffectPrefab, transform.position, Quaternion.identity, transform);
        UpdateBlueConeScale();
        weaponLevel = GameManager.Instance.meleeWeapon2_Level;
    }
    void Update()
    {
        if (isSkillActive)
        {
            // 스킬 활성화 또는 애니메이션 실행 중에는 부채꼴 방향 고정
            return;
        }

        // 조이스틱 입력
        ProcessInputs();
        weaponLevel = GameManager.Instance.meleeWeapon2_Level;
        MeleeLevel(weaponLevel);

        // 파란색 부채꼴 크기 업데이트
        UpdateBlueConeScale();

        // 파란색 부채꼴 위치 및 회전 업데이트
        UpdateBlueCone();

        // 공격 쿨타임 확인 후 공격
        if (Time.time >= lastAttackTime + attackCooldown && weaponLevel != 0)
        {
            lastAttackTime = Time.time;
            CastSkill();
        }
    }

    // 입력 처리
    void ProcessInputs()
    {
        float moveX = joystick.Horizontal;
        float moveY = joystick.Vertical;
        moveDirection = new Vector2(moveX, moveY).normalized;
    }

    // 파란색 부채꼴 회전 업데이트
    void UpdateBlueCone()
    {
        if (moveDirection.sqrMagnitude > 0)
        {
            // 조이스틱 입력 방향으로 부채꼴 회전
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            blueCone.transform.localRotation = Quaternion.Euler(0, 0, angle - skillAngle / 2); // 로컬 회전
        }
    }

    // 파란색 부채꼴 크기 설정 (스킬 범위에 맞게)
    void UpdateBlueConeScale()
    {
        if (blueCone != null)
        {
            blueCone.transform.localScale = new Vector3(scaleXY, scaleXY, 1);
        }
    }

   // 스킬 시전
    void CastSkill()
    {
        // 고정된 시작 각도 계산
        fixedStartAngle = blueCone.transform.localEulerAngles.z - skillAngle / 2;
        // 각도를 0~360도로 변환
        fixedStartAngle = Mathf.Repeat(fixedStartAngle, 360);

        // 고정된 끝 각도 계산
        fixedEndAngle = fixedStartAngle + skillAngle;
        fixedEndAngle = Mathf.Repeat(fixedEndAngle, 360);

        // Shader의 _StartAngle 속성을 초기화 및 설정
        timerMaterial.SetFloat("_StartAngle", fixedStartAngle - 6f);
        timerMaterial.SetFloat("_FillAngle", 0); // 초기화

        // 스킬 활성화 상태로 전환
        isSkillActive = true;

        // 붉은색 타이머 부채꼴 시작
        StartCoroutine(StartSkillTimer());
    }

    // 붉은색 타이머 부채꼴 실행
    IEnumerator StartSkillTimer()
    {
        float timer = 0f;

        while (timer < timerDuration)
        {
            // 타이머 진행에 따라 붉은색 타이머 부채꼴 각도 업데이트
            float fillAngle = Mathf.Lerp(0, skillAngle, timer / timerDuration);

            // 각도를 0~360도로 변환
            timerMaterial.SetFloat("_FillAngle", Mathf.Repeat(fillAngle + 14f, 360));

            timer += Time.deltaTime;
            yield return null;
        }

        // 타이머 종료 처리
        EndSkillTimer();
    }

    // 타이머 종료 처리
    void EndSkillTimer()
    {
        // 붉은색 부채꼴 타이머 비활성화
        timerMaterial.SetFloat("_FillAngle", 0);

        // 데미지 적용
        ApplyDamage();

        // 스킬 비활성화 상태로 전환
        isSkillActive = false;

        // 검을 부채꼴 끝 위치에서 시작 위치로 회전
        StartCoroutine(RotateSwordToStartAngle());
    }

   // 검 회전 코루틴
    IEnumerator RotateSwordToStartAngle()
    {
        if (swordObject == null) yield break;

        float rotationSpeed = 180f; // 회전 속도 (초당 회전 각도)
        float currentAngle = fixedEndAngle; // 현재 검 각도 (부채꼴 끝 위치)
        float targetAngle = Mathf.Repeat(currentAngle - 60f, 360f); // 목표 각도 (부채꼴 시작 위치)

        // 목표 회전 계산
        Quaternion startRotation = Quaternion.Euler(0, 0, currentAngle - 90f);
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle - 90f);

        // 검을 현재 각도로 설정 (부채꼴 끝 위치)
        swordObject.localRotation = startRotation;
        swordObject.localScale = new Vector3(scaleSwordXY, scaleSwordXY, 1);
        swordObject.gameObject.SetActive(true);

        // 회전 애니메이션
        while (Quaternion.Angle(swordObject.localRotation, targetRotation) > 0.1f)
        {
            swordObject.localRotation = Quaternion.RotateTowards(
                swordObject.localRotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
            yield return null;
        }

        // 최종적으로 목표 각도에 맞추기
        swordObject.localRotation = targetRotation;
        swordObject.gameObject.SetActive(false);
    }

    void OnDrawGizmos()
{
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, skillRadius);
}

    // 데미지 적용
    void ApplyDamage()
    {
        // OverlapCircleAll을 사용하여 범위 내 적 감지
        Collider2D[] hitTargets = Physics2D.OverlapCircleAll(transform.position, skillRadius, enemyLayer);

        foreach (Collider2D target in hitTargets)
        {

            Vector2 targetPosition = target.transform.position;

            // 부채꼴 범위 내 적인지 확인
            if (IsWithinCone(transform.position, targetPosition, blueCone.transform.right, skillAngle, skillRadius))
            {
                BaseEnemy enemy = target.GetComponent<BaseEnemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }
        }
    }


    bool IsWithinCone(Vector2 origin, Vector2 target, Vector2 direction, float angle, float radius)
    {
        Vector2 toTarget = target - origin;

        // 1. 거리가 범위 내에 있는지 확인
        if (toTarget.magnitude > radius) return false;

        // 2. 방향이 부채꼴 범위 내에 있는지 확인
        float dot = Vector2.Dot(direction.normalized, toTarget.normalized);
        float theta = Mathf.Acos(dot) * Mathf.Rad2Deg;

        return theta <= angle / 2; // 부채꼴 각도 범위 확인
    }

    /*void OnDisable()
    {
        // 스크립트가 비활성화될 때, blueCone이 자식으로 존재하고 활성화된 경우에만 삭제
        if (blueCone.transform.IsChildOf(transform) && blueCone.activeSelf)
        {
            Destroy(blueCone.gameObject); // 자식 오브젝트 삭제
            blueCone = null; // 참조 제거
        }
    }*/

    private void MeleeLevel(int level)
    {
        // 무기 레벨에 따른 공격 범위 및 데미지 설정
        switch (level)
        {
            case 0:
                scaleXY = 0f;
                scaleSwordXY = 0f;
                damage = 0;
                break;
            case 1:
                scaleXY = 0.15f;
                scaleSwordXY = 11.5f;
                damage = 15;
                break;
            case 2:
                scaleXY = 0.17f;
                scaleSwordXY = 13f;
                damage = 20;
                break;
            case 3:
                damage = 25;
                break;
            case 4:
                damage = 30;
                scaleXY = 0.2f;
                scaleSwordXY = 15f;
                break;
            case 5:
                damage = 35;
                break;
            case 6:
                scaleXY = 0.25f;
                scaleSwordXY = 19f;
                damage = 40;
                break;
            case 7:
                damage = 45;
                break;
            case 8:
                scaleXY = 0.3f;
                scaleSwordXY = 23f;
                damage = 50;
                break;
            default:
                break;
        }
    }
}