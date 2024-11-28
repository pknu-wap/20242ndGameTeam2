using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BossExplainAnimation : MonoBehaviour
{
    public GameObject Panel;
    public GameObject WeaponGet;
    void Awake()
    {
        Sequence sequence = DOTween.Sequence();

        RectTransform rectTransform = GetComponent<RectTransform>();
        sequence.Append((rectTransform.DOLocalMove(Vector3.zero, 1.8f, false).SetEase(Ease.OutExpo)));

        //sequence.AppendInterval(0.3f);

        sequence.Append((rectTransform.DOLocalMove(new Vector3(2308,-162,0), 2f, false).SetEase(Ease.InExpo)));

        Panel.SetActive(false);
        WeaponGet.SetActive(true);
        
    }
}
