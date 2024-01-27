using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class HoverScale : MonoBehaviour
{
    [SerializeField] private float duration;
    [SerializeField] private Vector3 targetScale;
    [SerializeField] private Ease ease;

    private Vector3 _startScale;
    private RectTransform _rect;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
    }

    private void Start()
    {
        _startScale = transform.localScale;
    }

    public void ScaleUp()
    {
        _rect.DOScale(targetScale, duration).SetEase(ease);
    }

    public void ScaleDown()
    {
        _rect.DOScale(_startScale, duration).SetEase(ease);
    }
}
