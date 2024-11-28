using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PuppetMarkMove : MonoBehaviour
{
    [SerializeField] private float moveValue = 0f;
    public SpriteRenderer spriteRenderer;  // 스프라이트를 변경할 SpriteRenderer
    public Sprite newSprite;   // 새로 사용할 Sprite
    void Awake()
    {
        transform.DOLocalMoveY(transform.localPosition.y - moveValue, 0.6f) // Y축으로 3단위 만큼 위로 이동
            .SetLoops(-1, LoopType.Yoyo) // 무한 반복, 위아래 반복
            .SetEase(Ease.InOutSine); // 부드럽게 위아래로 움직임
    }

    public void ChangeSprite()
    {
        if (spriteRenderer != null && newSprite != null)
        {
            spriteRenderer.sprite = newSprite; // 스프라이트 변경
        }
        
    }
}
