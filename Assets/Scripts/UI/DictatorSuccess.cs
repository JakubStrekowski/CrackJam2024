using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DictatorSuccess : MonoBehaviour
{
    private DictatorChan currentDictator;
    private Image _image;
    
    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void SetDictatorSuccess(DictatorChan currantDictator)
    {
        currentDictator = currantDictator;
        StartCoroutine(nameof(CharacterSuccessSkinController));

        if (currantDictator.name == "KimDictator" || currantDictator.name == "Janina Pawelska")
        {
            GetComponent<BreatheEffect>().targetScale = new Vector3(0.71f, 0.725f, 0.7f);
            GetComponent<BreatheEffect>().RecalculateSequence(new Vector3(0.7f, 0.7f, 0.7f));
            transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        }
        if (currantDictator.name == "Janina Pawelska")
        {
            GetComponent<BreatheEffect>().targetScale = new Vector3(1.35f, 1.375f, 1.35f);
            GetComponent<BreatheEffect>().RecalculateSequence(new Vector3(1.35f, 1.35f, 1.35f));
            transform.localScale = new Vector3(1.35f, 1.35f, 1.35f);
        }
        
    }
    
    private IEnumerator CharacterSuccessSkinController()
    {
        while (true)
        {
            currentDictator.successSkinId = (currentDictator.successSkinId + 1) % currentDictator.successSkin.Length;
                
            _image.sprite = currentDictator
                .successSkin[currentDictator.successSkinId];

            yield return new WaitForSeconds(0.25f);
        }
    }
    
    public void SetDictatorFailure(DictatorChan currantDictator)
    {
        currentDictator = currantDictator;
        if (currantDictator.name == "KimDictator")
        {
            StartCoroutine(nameof(KimFailureSkinController));
        }
        else
        {
            StartCoroutine(nameof(CharacterFailureSkinController));
        }

        if (currantDictator.name == "KimDictator" || currantDictator.name == "Janina Pawelska" || currantDictator.name == "AdaHitlerina")
        {
            GetComponent<BreatheEffect>().targetScale = new Vector3(0.71f, 0.725f, 0.7f);
            GetComponent<BreatheEffect>().RecalculateSequence(new Vector3(0.7f, 0.7f, 0.7f));
            transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        }
    }
    
    private IEnumerator CharacterFailureSkinController()
    {
        while (true)
        {
            currentDictator.failureSkinId = (currentDictator.failureSkinId + 1) % currentDictator.failureSkin.Length;
                
            _image.sprite = currentDictator
                .failureSkin[currentDictator.failureSkinId];

            yield return new WaitForSeconds(0.25f);
        }
    }
    
    private IEnumerator KimFailureSkinController()
    {
        currentDictator.failureSkinId = -1;
        
        while (currentDictator.failureSkinId < currentDictator.failureSkin.Length - 1)
        {
            currentDictator.failureSkinId = (currentDictator.failureSkinId + 1);

            _image.sprite = currentDictator
                .failureSkin[currentDictator.failureSkinId];
            
            if (currentDictator.failureSkinId == 0)
            {
                yield return new WaitForSeconds(0.5f);
            }

            yield return new WaitForSeconds(0.25f);
        }
    }
}
