using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Boss1BlockMove : MonoBehaviour
{
    [SerializeField] GameObject boss1Block;
    [SerializeField] GameObject boss1Object;
    [SerializeField] GameObject healthBar1;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Player"))
        {
            boss1Block.transform.DOLocalMove(new Vector3(3.34f, 30.29f, 9.843f), 0.5f);
            boss1Object.SetActive(true);
            healthBar1.SetActive(true);
        }
        
    }
}
