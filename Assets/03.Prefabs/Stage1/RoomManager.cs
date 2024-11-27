using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    // Singleton 인스턴스
    public static RoomManager Instance { get; private set; }

    public GameObject roomPrefab;
    public GameObject doorPrefab;
    public int maxRooms = 5;

    private Dictionary<Vector2Int, GameObject> rooms = new Dictionary<Vector2Int, GameObject>();
    private GameObject player;

    private void Awake()
    {
        // Singleton 초기화
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // 이미 인스턴스가 존재하면 삭제
        }
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        GenerateRooms();
    }

    // 방 생성 로직 등은 기존 코드 그대로 유지
    void GenerateRooms()
    {
        Vector2Int currentPos = Vector2Int.zero;
        Queue<Vector2Int> roomQueue = new Queue<Vector2Int>();
        roomQueue.Enqueue(currentPos);

        while (rooms.Count < maxRooms && roomQueue.Count > 0)
        {
            Vector2Int roomPos = roomQueue.Dequeue();

            if (rooms.ContainsKey(roomPos)) continue;

            GameObject room = Instantiate(roomPrefab, (Vector3Int)roomPos * 10, Quaternion.identity);
            rooms.Add(roomPos, room);

            foreach (Vector2Int dir in GetDirections())
            {
                Vector2Int newRoomPos = roomPos + dir;
                if (!rooms.ContainsKey(newRoomPos) && Random.value > 0.5f)
                {
                    roomQueue.Enqueue(newRoomPos);
                    ConnectRooms(room, roomPos, dir);
                }
            }
        }
    }

    void ConnectRooms(GameObject room, Vector2Int roomPos, Vector2Int direction)
    {
        GameObject door = Instantiate(doorPrefab, room.transform);
        door.transform.position = room.transform.position + (Vector3)(Vector2)direction * 5;
        door.GetComponent<Door>().connectedRoomPosition = roomPos + direction;
    }

    List<Vector2Int> GetDirections()
    {
        return new List<Vector2Int> { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
    }

    public void MoveToRoom(Vector2Int roomPos)
    {
        if (rooms.ContainsKey(roomPos))
        {
            player.transform.position = rooms[roomPos].transform.position;
        }
    }
}