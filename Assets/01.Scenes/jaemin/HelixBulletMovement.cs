using UnityEngine;

public class HelixBulletMovement : MonoBehaviour
{
    private Vector3 center;         // 총알이 시작되는 중심
    private float speed;            // 총알 속도
    private float radiusIncrement;  // 반지름 증가 속도
    private float angleSpeed;       // 각도 변화 속도
    private float lifetime;         // 총알의 수명 -- 이것들 전부 HellixAttack에서 설정된 것들

    private float currentRadius = 0f; // 현재 반지름
    private float currentAngle = 0f;  // 현재 각도

    public void Initialize(Vector3 center, float speed, float radiusIncrement, float angleSpeed, float lifetime)
    {
        this.center = center; // self 썻다가 안돼서 당황함 ㅋㅋ
        this.speed = speed;
        this.radiusIncrement = radiusIncrement;
        this.angleSpeed = angleSpeed;
        this.lifetime = lifetime;

        Destroy(gameObject, lifetime); // 총알 유지시간 끝나면 삭제
    }

    void Update()
    {
        // 반지름과 각도 갱신
        currentRadius += radiusIncrement * Time.deltaTime;
        currentAngle += angleSpeed * Time.deltaTime;

        // 극좌표 계산
        float x = center.x + currentRadius * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
        float y = center.y + currentRadius * Mathf.Sin(currentAngle * Mathf.Deg2Rad);

        // 총알의 위치 갱신
        transform.position = new Vector3(x, y, 0);

        // 총알 이동 방향 계산
        Vector3 direction = (transform.position - center).normalized;
        transform.up = direction; // 총알이 회전하며 이동
    }
}