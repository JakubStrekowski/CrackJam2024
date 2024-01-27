using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BreatheEffect : MonoBehaviour
{
    [SerializeField] private Vector3 targetScale = Vector3.one;
    [SerializeField] private float duration = 2.4f;
    [SerializeField] private Ease ease;
    
    private Sequence _seq;
    private RectTransform _rect;
    
    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        _seq = DOTween.Sequence();
        _seq.Append(_rect.DOScale(targetScale, duration).SetEase(ease));
        _seq.Append(_rect.DOScale(Vector3.one, duration).SetEase(ease));
        _seq.AppendInterval(0.4f);
        _seq.SetLoops(-1);
        _seq.Play();
    }
}