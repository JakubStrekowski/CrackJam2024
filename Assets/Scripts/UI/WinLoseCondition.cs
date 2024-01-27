using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using DG.Tweening;
using TMPro;
using UnityEngine;

public enum EGameFinishState
{
    None,
    Success,
    Failure
}

public class WinLoseCondition : MonoBehaviour
{
    [SerializeField] private GameObject winUi;
    [SerializeField] private GameObject loseUi;
    [SerializeField] private TMP_Text timeText;
    [Space]
    [SerializeField] private EGameFinishState state;

    public float startTime = 5;
    public float animTime;
    public Ease animEase;

    private float _currentTime;

    private void Start()
    {
        StartCountdown();
    }

    public void SetFinishState(EGameFinishState value)
    {
        switch (value)
        {
            default:
            case EGameFinishState.None:
                break;
            case EGameFinishState.Success:
                StopCoroutine(nameof(CountDown));
                OnGameWin();
                break;
            case EGameFinishState.Failure:
                OnGameLose();
                break;
        }
    }

    public void StartCountdown()
    {
        _currentTime = startTime;
        timeText.text = _currentTime.ToString(CultureInfo.InvariantCulture);
        StartCoroutine(nameof(CountDown));
    }

    private IEnumerator CountDown()
    {
        while (_currentTime > 0)
        {
            _currentTime -= Time.deltaTime;
            timeText.text = _currentTime.ToString(CultureInfo.InvariantCulture);
            yield return null;
        }
        
        SetFinishState(EGameFinishState.Failure);
    }
    

    private void OnGameWin()
    {
        winUi.SetActive(true);
        winUi.transform.localScale = Vector3.zero;
        winUi.transform.DOScale(Vector3.one, animTime).SetEase(animEase);
    }
    
    private void OnGameLose()
    {
        loseUi.SetActive(true);
        loseUi.transform.localScale = Vector3.zero;
        loseUi.transform.DOScale(Vector3.one, animTime).SetEase(animEase);
    }
    
}
