using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDirection : MonoBehaviour
{
    private Vector2 moveDirection;
    public Joystick joystick;
    public Transform player; // 플레이어의 Transform을 받을 변수

    void Update()
    {
        ProcessInputs();
        MovePoint();
    }

    void ProcessInputs()
    {
        float moveX = joystick.Horizontal;
        float moveY = joystick.Vertical;

        
        moveDirection = new Vector2(moveX, moveY).normalized; // 입력 방향을 정규화
        
    }

    void MovePoint()
    {
        // 조이스틱 입력이 있을 때만 위치를 이동
        if (moveDirection != Vector2.zero)
        {
            // 플레이어 위치에서 moveDirection을 더하여 이동
            transform.position = player.position + (Vector3)moveDirection; // 플레이어 위치에 moveVector를 더하여 이동
        }
    }
}
