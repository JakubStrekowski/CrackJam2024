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
            GetComponent<BreatheEffect>().RecalculateSequence();
            transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
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
}
