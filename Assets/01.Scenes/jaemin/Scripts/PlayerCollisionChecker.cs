using UnityEngine;

public class PlayerCollisionChecker : MonoBehaviour
{
    public float checkRadius = 0.5f; // 충돌을 검사할 반경
    public LayerMask wallLayer;

    // 이동할 위치가 벽과 충돌하지 않는지 확인하는 메서드
    public bool CanMoveTo(Vector2 targetPosition)
    {
        Collider2D hit = Physics2D.OverlapCircle(targetPosition, checkRadius, wallLayer);

        return hit == null; // 벽이 없으면 true, 벽이 있으면 false 반환
    }

    // 시각적 확인을 위한 Gizmos
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}