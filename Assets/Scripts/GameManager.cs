using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    [SerializeField] private DictatorChan[] dictators;
    int currentDictator = 0;
    int lovePoints = 0;
    Sentence currentSentence;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI dictatorName;
    [SerializeField] private TextMeshProUGUI question;
    [SerializeField] private Button[] dialogButtons;

    private void Start()
    {
        for(int i = 0; i < dialogButtons.Length; i++)
        {
            int a = i;
            dialogButtons[a].onClick.AddListener(() => SelectDialogOption(a));
        }
        HideButtons(false);
        //NA POTRZEBY TESTU
        LoadNewDictator(0);
    }

    void HideButtons(bool hide)
    {
        for (int i = 0; i < dialogButtons.Length; i++)
        {
            dialogButtons[i].gameObject.SetActive(hide);
        }
    }

    public void SelectDialogOption(int id)
    {
        Debug.Log(id);
        id = Mathf.Clamp(id, 0, currentSentence.GetAnswersCount()-1);
        lovePoints += currentSentence.GetSentence(id).GetAnswetValue();
        LoadDialog();
    }

    [ContextMenu("LoadDialog")]
    public void LoadDialog()
    {
        if (!dictators[currentDictator].IsMoreSentences())
        {
            Debug.Log("No more sentences!");
            EndGame();
            return;
        }

        currentSentence = dictators[currentDictator].GetNextSentence();
        question.SetText(currentSentence.GetDescription());
        for (int i = 0; i < dialogButtons.Length && i< currentSentence.GetAnswersCount(); i++)
        {
            //update buttons decriptions
            dialogButtons[i].gameObject.SetActive(true);
            dialogButtons[i].transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().SetText(currentSentence.GetSentence(i).GetAnswerText());
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
        dictatorName.SetText(dictators[currentDictator].GetDictatorName());
        LoadDialog();
    }

    private void EndGame()
    {
        Debug.Log("Game ended!");
        HideButtons(false);
    }
}
