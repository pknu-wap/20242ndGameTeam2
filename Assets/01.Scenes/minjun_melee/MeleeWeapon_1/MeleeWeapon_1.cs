using System.Collections;
using UnityEngine;
using UnityEditor;

public class MeleeWeapon_1 : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] private int weaponLevel = 0;         // 무기 레벨
    [SerializeField] private int damage = 10;            // 기본 공격력
    [SerializeField] private float attackCooldown = 0.5f;   // 공격 딜레이


    [Header("Skill Parameters")]
    [SerializeField] private float skillRadius = 5f; // 스킬 최대 거리
    [SerializeField] private float innerRadius = 3f; // 중심부 거리
    [SerializeField] private float skillAngle = 60f; // 부채꼴 각도
    [SerializeField] private float delayBeforeDamage = 0.5f; // 부채꼴 이펙트 표시 후 데미지 적용 시간

    [Header("Effect Settings")]
    public GameObject coneEffectPrefab; // 부채꼴 이펙트 프리팹
    public LayerMask enemyLayer; // 적 레이어 마스크

    [Header("Debug")]
    private Vector2 moveDirection;
    public Joystick joystick;

    private GameObject activeEffect;
    private float lastAttackTime;        // 마지막 공격 시간 기록

    void Update()
    {

        // 공격 쿨타임 체크 및 공격 트리거
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            MeleeLevel(weaponLevel);

            //조이스틱 방향 받아오기
            ProcessInputs();
            CastSkill();
        }
    }
    void ProcessInputs()
    {
        float moveX = joystick.Horizontal;
        float moveY = joystick.Vertical;

        moveDirection = new Vector2(moveX, moveY).normalized; // 입력 방향을 정규화
    }

    void CastSkill()
    {
        Vector3 origin = transform.position;
        Vector3 direction = moveDirection;

        // 부채꼴 이펙트를 생성
        ShowSkillEffect(origin, direction);

        // 일정 시간 후 적 감지 및 데미지 적용
        Invoke(nameof(ApplyDamage), delayBeforeDamage);
    }

    void ShowSkillEffect(Vector3 origin, Vector3 direction)
    {
        if (coneEffectPrefab == null) return;

        // 부채꼴 이펙트 생성
        activeEffect = Instantiate(coneEffectPrefab, origin, Quaternion.identity);
        activeEffect.transform.forward = direction;

        // 부채꼴 크기 조정
        float scaleX = skillRadius / 5f; // 프리팹 기본 크기에 맞춰 조정
        float scaleY = skillAngle / 90f; // 각도에 맞춰 조정
        activeEffect.transform.localScale = new Vector3(scaleX, 1, scaleY);
    }

    void ApplyDamage()
    {
        if (activeEffect != null)
        {
            Destroy(activeEffect); // 부채꼴 이펙트 제거
        }

        Vector3 origin = transform.position;
        Vector3 direction = transform.forward;

        // 반경 내의 적을 탐색
        Collider[] hitTargets = Physics.OverlapSphere(origin, skillRadius, enemyLayer);

        foreach (Collider target in hitTargets)
        {
            Vector3 targetPosition = target.transform.position;

            // 부채꼴 범위 내 적인지 확인
            if (IsWithinCone(origin, targetPosition, direction, skillAngle, skillRadius))
            {
                float distance = Vector3.Distance(origin, targetPosition);

                if (distance <= innerRadius)
                {
                    // 중심부 효과
                    Debug.Log($"Center Effect Applied to {target.name}");
                }
                else
                {
                    // 가장자리 효과
                    Debug.Log($"Outer Effect Applied to {target.name}");
                }
            }
        }
    }

    bool IsWithinCone(Vector3 origin, Vector3 target, Vector3 direction, float angle, float radius)
    {
        Vector3 toTarget = target - origin;
        if (toTarget.magnitude > radius) return false; // 거리 초과

        float dot = Vector3.Dot(direction.normalized, toTarget.normalized);
        float theta = Mathf.Acos(dot) * Mathf.Rad2Deg; // 각도 계산

        return theta <= angle / 2;
    }

    private void MeleeLevel(int level)
    {
        /*
        // 무기 레벨에 따른 공격 범위 및 데미지 설정
        switch (level)
        {
            case 1:
                attackRange_x = 5;
                attackRange_y = 2;
                break;
            case 3:
                damage = 15;
                break;
            case 4:
                animator.runtimeAnimatorController = overrideController_Level4;
                damage = 20;
                attackRange_x = 5.5f;
                attackRange_y = 2.5f;
                break;
            case 5:
                damage = 25;
                break;
            case 6:
            animator.runtimeAnimatorController = overrideController_Level6;
                attackRange_x = 6;
                attackRange_y = 3f;
                damage = 30;
                break;
            case 7:
                damage = 35;
                break;
            case 8:
                damage = 40;
                break;
            default:
                break;
        }*/
    }

void OnDrawGizmos()
{
    Vector3 origin = transform.position;

    // 로컬 Z축을 기준으로 회전 (로컬 좌표 기준)
    Vector3 localForward = moveDirection;/*transform.TransformDirection(Vector3.up);*/ // 로컬 Z축을 월드 좌표로 변환

    // 기즈모 색상 설정
    Gizmos.color = new Color(1, 0, 0, 0.5f); // 빨간색, 반투명

    // 부채꼴 그리기
    DrawSectorGizmo(origin, localForward, skillRadius, skillAngle);

    // 중심부 범위 그리기
    Gizmos.color = new Color(0, 1, 0, 0.5f); // 초록색, 반투명
    Gizmos.DrawWireSphere(origin, innerRadius);
}

void DrawSectorGizmo(Vector3 origin, Vector3 direction, float radius, float angle)
{
    int segments = 50; // 부채꼴을 구성하는 선분의 개수
    float halfAngle = angle / 2;

    // 부채꼴의 시작점 계산
    Vector3 previousPoint = origin + Quaternion.Euler(0, 0, -halfAngle) * direction * radius;

    for (int i = 1; i <= segments; i++)
    {
        // 현재 각도를 계산
        float currentAngle = -halfAngle + (angle / segments) * i;

        // 현재 각도에서의 점 계산
        Vector3 currentPoint = origin + Quaternion.Euler(0, 0, currentAngle) * direction * radius;

        // 이전 점과 현재 점을 연결
        Gizmos.DrawLine(previousPoint, currentPoint);

        // 부채꼴 중심에서 현재 점으로 선 그리기 (선택 사항)
        Gizmos.DrawLine(origin, currentPoint);

        previousPoint = currentPoint; // 다음 점을 위해 갱신
    }

    // 마지막 점과 부채꼴 시작점을 연결 (부채꼴 닫기)
    Gizmos.DrawLine(previousPoint, origin + Quaternion.Euler(0, 0, -halfAngle) * direction * radius);
}

}
