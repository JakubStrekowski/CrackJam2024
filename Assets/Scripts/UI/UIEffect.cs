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
    [SerializeField] private float startDelay;
    [SerializeField] private float duration;
    [SerializeField] private Ease ease;
    [SerializeField] private bool isEntranceOnly;
    [Space]
    [SerializeField] private Vector3 rotation;
    [SerializeField] private Vector3 scale;
    [SerializeField] private Vector3 position;
    [SerializeField] private Color color;
    
    private Sequence _seq;
    private RectTransform _rect;
    private Image _renderer;
    
    private Vector3 _startRotation;
    private Vector3 _startScale;
    private Vector3 _startPosition;
    private Color _startColor;
    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
        _renderer = GetComponent<Image>();
    }

    public void Start()
    {
        _startRotation = _rect.rotation.eulerAngles;
        _startScale = _rect.localScale;
        _startPosition = _rect.anchoredPosition;
        _startColor = _renderer.color;
        _seq = DOTween.Sequence();
        EvaluateEffect();
    }

    private void EvaluateEffect()
    {
        if (effect.HasFlag(EUiEffect.Rotate))
        {
            _seq.Insert(startDelay, _rect.DORotate(rotation, duration).SetEase(ease));
            if (!isEntranceOnly)
            {
                _seq.Insert(startDelay + duration, _rect.DORotate(_startRotation, duration).SetEase(ease));
            }
        }
        if (effect.HasFlag(EUiEffect.Rescale))
        {
            _seq.Insert(startDelay, transform.DOScale(scale, duration).SetEase(ease));
            if (!isEntranceOnly)
            {
                _seq.Insert(startDelay + duration, transform.DOScale(_startScale, duration).SetEase(ease));
            }
        }
        if (effect.HasFlag(EUiEffect.Move))
        {
            _seq.Insert(startDelay, _rect.DOAnchorPos(position, duration).SetEase(ease));
            if (!isEntranceOnly)
            {
                _seq.Insert(startDelay + duration, _rect.DOAnchorPos(_startPosition, duration).SetEase(ease));
            }
        }
        if (effect.HasFlag(EUiEffect.Recolor))
        {
            _seq.Insert(startDelay,_renderer.DOColor(color, duration).SetEase(ease));
            if (!isEntranceOnly)
            {
                _seq.Insert(startDelay + duration,_renderer.DOColor(_startColor, duration).SetEase(ease));
            }
        }

        if (!isEntranceOnly)
        {
            _seq.SetLoops(-1);
        }
        _seq.Play();
    }
}
