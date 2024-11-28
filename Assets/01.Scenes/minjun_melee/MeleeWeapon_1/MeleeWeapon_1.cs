using System.Collections;
using UnityEngine;

public class MeleeWeapon_1 : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] private int weaponLevel = 0; // 무기 레벨
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

    [Header("Debug")]
    public Joystick joystick; // 조이스틱 입력
    private Vector2 moveDirection; // 입력 방향

    private GameObject blueCone; // 파란색 부채꼴
    private float fixedStartAngle; // 고정된 시작 각도
    private bool isSkillActive = false; // 스킬 활성화 상태
    private float lastAttackTime; // 마지막 공격 시간


    float scaleX = 0f;
    float scaleY = 0f;

    void Start()
    {
        // 붉은색 타이머 초기화
        timerMaterial.SetFloat("_StartAngle", 0);
        timerMaterial.SetFloat("_FillAngle", 0);

        // 파란색 부채꼴 생성 및 부모 설정
        blueCone = Instantiate(coneEffectPrefab, transform.position, Quaternion.identity, transform); // 부모를 플레이어로 설정
        UpdateBlueConeScale();
    }

    void Update()
    {
        // 조이스틱 입력
        ProcessInputs();
        MeleeLevel(weaponLevel);

        // 파란색 부채꼴 크기 업데이트
        UpdateBlueConeScale();

        Debug.Log("스케일 x :" + scaleX + "스케일 y : " + scaleY);
        // 파란색 부채꼴 위치 및 회전 업데이트
        if (!isSkillActive)
        {
            UpdateBlueCone();
        }

        // 공격 쿨타임 확인 후 공격
        if (Time.time >= lastAttackTime + attackCooldown)
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
            Debug.Log("스케일 x :" + scaleX + "스케일 y : " + scaleY);
            blueCone.transform.localScale = new Vector3(scaleX, scaleY, 1);
        }
    }

    // 스킬 시전
    void CastSkill()
    {
        // 고정된 시작 각도 계산
        fixedStartAngle = blueCone.transform.localEulerAngles.z - skillAngle / 2;

        // Shader의 `_StartAngle` 속성을 초기화 및 설정
        timerMaterial.SetFloat("_StartAngle", fixedStartAngle);
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
            timerMaterial.SetFloat("_FillAngle", fillAngle);

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
    }

    // 데미지 적용
    void ApplyDamage()
    {
        Collider[] hitTargets = Physics.OverlapSphere(transform.position, skillRadius, enemyLayer);

        foreach (Collider target in hitTargets)
        {
            Vector3 targetPosition = target.transform.position;

            // 부채꼴 범위 내 적인지 확인
            if (IsWithinCone(transform.position, targetPosition, blueCone.transform.right, skillAngle, skillRadius))
            {
                target.GetComponent<BaseEnemy>().TakeDamage(damage);
            }
        }
    }

    // 부채꼴 범위 내인지 확인
    bool IsWithinCone(Vector3 origin, Vector3 target, Vector3 direction, float angle, float radius)
    {
        Vector3 toTarget = target - origin;
        if (toTarget.magnitude > radius) return false;

        float dot = Vector3.Dot(direction.normalized, toTarget.normalized);
        float theta = Mathf.Acos(dot) * Mathf.Rad2Deg;

        return theta <= angle / 2;
    }

    // 디버그: 기즈모로 부채꼴 표시
    void OnDrawGizmos()
    {
        if (isSkillActive)
        {
            Gizmos.color = new Color(1, 0, 0, 0.5f); // 붉은색
            DrawSectorGizmo(transform.position, blueCone.transform.right, skillRadius, skillAngle);
        }
        else
        {
            Gizmos.color = new Color(0, 0, 1, 0.2f); // 파란색
            DrawSectorGizmo(transform.position, moveDirection, skillRadius, skillAngle);
        }
    }

    // 부채꼴 기즈모 그리기
    void DrawSectorGizmo(Vector3 origin, Vector3 direction, float radius, float angle)
    {
        int segments = 50;
        float halfAngle = angle / 2;

        Vector3 previousPoint = origin + Quaternion.Euler(0, 0, -halfAngle) * direction * radius;

        for (int i = 1; i <= segments; i++)
        {
            float currentAngle = -halfAngle + (angle / segments) * i;
            Vector3 currentPoint = origin + Quaternion.Euler(0, 0, currentAngle) * direction * radius;

            Gizmos.DrawLine(previousPoint, currentPoint);
            Gizmos.DrawLine(origin, currentPoint);

            previousPoint = currentPoint;
        }

        Gizmos.DrawLine(previousPoint, origin + Quaternion.Euler(0, 0, -halfAngle) * direction * radius);
    }

     private void MeleeLevel(int level)
    {
        // 무기 레벨에 따른 공격 범위 및 데미지 설정
        switch (level)
        {
            case 1:
                scaleX = 0.3f;
                scaleY = 0.3f;
                break;
            case 3:
                damage = 15;
                break;
            case 4:
                damage = 20;
                scaleX = 0.35f;
                scaleY = 0.35f;
                break;
            case 5:
                damage = 25;
                break;
            case 6:
                scaleX = 0.4f;
                scaleY = 0.4f;
                damage = 30;
                break;
            case 7:
                damage = 35;
                break;
            case 8:
                scaleX = 0.5f;
                scaleY = 0.5f;
                damage = 40;
                break;
            default:
                break;
        }
    }
}
