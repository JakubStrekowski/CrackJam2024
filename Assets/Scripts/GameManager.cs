using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private DictatorChan[] dictators;
    int currentDictator = 0;
    int lovePoints = 0;
    Sentence currentSentence;

    public void SelectDialogOption(int id)
    {
        id = Mathf.Clamp(id, 0, currentSentence.GetAnswersCount()-1);
        lovePoints += currentSentence.GetSentence(id).GetAnswetValue();
    }

    [ContextMenu("LoadDialog")]
    public void LoadDialog()
    {
        if (!dictators[currentDictator].IsMoreSentences())
        {
            Debug.Log("No more sentences!");
            return;
        }

        currentSentence = dictators[currentDictator].GetNextSentence();
        //update buttons decriptions
        Debug.Log(currentSentence.GetDescription());
        for (int i = 0; i < 3; i++)
        {
            //update buttons decriptions
            Debug.Log(currentSentence.GetSentence(i));
        }

    }

    [ContextMenu("LoadNewDictator")]
    public void LoadNewDictator()
    {
        LoadNewDictator(++currentDictator);
    }

    public void LoadNewDictator(int i)
    {
        currentDictator = Mathf.Clamp(i, 0, dictators.Length-1);
        dictators[currentDictator].PrepreDictator();
        //set visible name
        LoadDialog();
    }

}
