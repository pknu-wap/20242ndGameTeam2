using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Boss2BlockMove : MonoBehaviour
{
    [SerializeField] GameObject boss2Block;
    [SerializeField] GameObject boss2Object;
    [SerializeField] GameObject healthBar2;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player"))
        {
            boss2Block.transform.DOLocalMove(new Vector3(3.35f, 65.48f, 9.843299f), 0.5f);
            boss2Object.SetActive(true);
            healthBar2.SetActive(true);
        }
        
    }
}
