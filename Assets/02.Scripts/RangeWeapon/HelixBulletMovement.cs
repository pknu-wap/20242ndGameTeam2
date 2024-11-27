using UnityEngine;

public class HelixBulletMovement : MonoBehaviour
{
    private Transform center;       // 회전 중심 (플레이어 위치)
    private float speed;            // 총알 속도
    private float radiusIncrement;  // 반지름 증가 속도
    private float angleSpeed;       // 각도 변화 속도
    private float lifetime;         // 총알 수명
    private float currentRadius = 0f; // 현재 반지름
    private float currentAngle = 0f;  // 현재 각도
    private int bulletIndex;         // 총알의 인덱스
    private int bulletCount;         // 발사된 총알 개수

    public void Initialize(Transform center, float speed, float radiusIncrement, float angleSpeed, float lifetime, int bulletIndex, int bulletCount)
    {
        this.center = center; // 플레이어를 중심으로 회전
        this.speed = speed;
        this.radiusIncrement = radiusIncrement;
        this.angleSpeed = angleSpeed;
        this.lifetime = lifetime;
        this.bulletIndex = bulletIndex;
        this.bulletCount = bulletCount;

        Destroy(gameObject, lifetime); // 수명 종료 시 총알 제거

        // 초기 각도 설정 (총알이 각도에 따라 나뉘어 발사)
        currentAngle = (360f / bulletCount) * bulletIndex;
    }

    void Update()
    {
        // 반지름과 각도 갱신
        currentRadius += radiusIncrement * Time.deltaTime;
        currentAngle += angleSpeed * Time.deltaTime;

        // 극좌표 계산
        float x = center.position.x + currentRadius * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
        float y = center.position.y + currentRadius * Mathf.Sin(currentAngle * Mathf.Deg2Rad);

        // 총알 위치 갱신
        transform.position = new Vector3(x, y, 0);

        // 총알 이동 방향 계산
        Vector3 direction = (transform.position - center.position).normalized;
        transform.up = direction; // 총알이 회전하며 이동
    }
}