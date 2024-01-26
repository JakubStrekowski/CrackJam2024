using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[Flags]
public enum EUiEffect
{
    Rotate = 0x01,
    Rescale = 0x02,
    Move = 0x04,
    Recolor = 0x08
}
public class UIEffect : MonoBehaviour
{
    [SerializeField] private EUiEffect effect;
    [SerializeField] private float duration;
    [SerializeField] private Ease ease;
    [Space]
    [SerializeField] private Vector3 rotation;
    [SerializeField] private Vector3 scale;
    [SerializeField] private Vector3 position;
    [SerializeField] private Color color;
    
    private Sequence _seq;
    private RectTransform _rect;
    private Image _renderer;
    
    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _renderer = GetComponent<Image>();
    }

    public void Start()
    {
        _seq = DOTween.Sequence();
        EvaluateEffect();
    }

    private void EvaluateEffect()
    {
        if (effect.HasFlag(EUiEffect.Rotate))
        {
            _seq.Insert(0, _rect.DORotate(rotation, duration).SetEase(ease));
        }
        if (effect.HasFlag(EUiEffect.Rescale))
        {
            _seq.Insert(0, transform.DOScale(scale, duration).SetEase(ease));
        }
        if (effect.HasFlag(EUiEffect.Move))
        {
            _seq.Insert(0, _rect.DOAnchorPos(position, duration).SetEase(ease));
        }
        if (effect.HasFlag(EUiEffect.Recolor))
        {
            _seq.Insert(0,_renderer.DOColor(color, duration).SetEase(ease));
        }

        _seq.SetLoops(-1);
        _seq.Play();
    }
}
