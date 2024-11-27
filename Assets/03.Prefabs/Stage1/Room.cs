using UnityEngine;

public class Room : MonoBehaviour
{
    public Color roomColor; // 방을 구별하기 위한 색상
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = roomColor; // 방에 색상을 입혀 구별 가능
    }
}