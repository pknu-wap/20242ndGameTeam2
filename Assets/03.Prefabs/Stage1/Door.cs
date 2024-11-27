using UnityEngine;

public class Door : MonoBehaviour
{
    public Vector2Int connectedRoomPosition;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // RoomManager의 Singleton Instance를 통해 방 이동
            RoomManager.Instance.MoveToRoom(connectedRoomPosition);
        }
    }
}