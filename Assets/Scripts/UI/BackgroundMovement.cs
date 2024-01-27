using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundMovement : MonoBehaviour
{
    [SerializeField] private float xSpeed;
    [SerializeField] private float ySpeed;
    
    private RawImage _rawImage;
    private float _currentX;
    private float _currentY;

    [SerializeField] private bool _isClosed;
    private void Awake()
    {
        _rawImage = GetComponent<RawImage>();
    }

    private void OnEnable()
    {
        StartCoroutine(nameof(MoveTexture));
    }

    private void OnDisable()
    {
        StopCoroutine(nameof(MoveTexture));
    }

    private IEnumerator MoveTexture()
    {
        while (!_isClosed)
        {
            _currentX += (xSpeed * Time.deltaTime);
            _currentY = Mathf.Sin(Time.time * ySpeed);
            _rawImage.uvRect = new Rect(_currentX, _currentY, _rawImage.uvRect.width, _rawImage.uvRect.height);
            
            yield return null;
        }
    }
}
