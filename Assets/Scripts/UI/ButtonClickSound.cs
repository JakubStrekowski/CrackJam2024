using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonClickSound : MonoBehaviour, IPointerClickHandler
{
    
    private AudioSource _as;
    private void Awake()
    {
        _as = GameObject.FindGameObjectsWithTag("AudioSource")[0].GetComponent<AudioSource>();
        _as.loop = false;
        _as.playOnAwake = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _as.PlayOneShot(_as.clip);
        Debug.Log("click");
    }
}
