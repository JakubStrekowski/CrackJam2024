using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Image dictatorRepresentation; 
    [SerializeField] private Image backgroundRepresentation; 
    [SerializeField] private GlobalSettings globalSettings;
    [SerializeField] private DictatorChan[] dictators;
    int currentDictator = 0;
    int lovePoints = 2;
    int winCap = 10;
    Sentence currentSentence;

    [Header("Panels")]
    [SerializeField] private GameObject gameplayPanel;
    [SerializeField] private GameObject resultPanel;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI dictatorName;
    [SerializeField] private TextMeshProUGUI question;
    [SerializeField] private Button[] dialogButtons;
    [SerializeField] private float estimatedTimeToWriteOut = 5.0f;
    [Space]
    [SerializeField] private TextMeshProUGUI resultOutput;

    private void Start()
    {
        PlayerPrefs.SetInt("Agreed",2137);
        gameplayPanel.SetActive(true);
        resultPanel.SetActive(false);

        for(int i = 0; i < dialogButtons.Length; i++)
        {
            int a = i;
            dialogButtons[a].onClick.AddListener(() => SelectDialogOption(a));
        }

        HideButtons(false);
        LoadNewDictator(globalSettings.choosenWaifu);
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
        id = Mathf.Clamp(id, 0, currentSentence.GetAnswersCount()-1);
        lovePoints += currentSentence.GetSentence(id).GetAnswetValue();
        if (lovePoints >= winCap) EndGame(MeetingResult.Good);
        else if (lovePoints <= 0) EndGame(MeetingResult.Bad);
        LoadDialog();
    }

    [ContextMenu("LoadDialog")]
    public void LoadDialog()
    {
        if (!dictators[currentDictator].IsMoreSentences())
        {
            Debug.Log("No more sentences!");
            EndGame(MeetingResult.Neutral);
            return;
        }

        currentSentence = dictators[currentDictator].GetNextSentence();
        question.SetText(currentSentence.GetDescription());
        StopAllCoroutines();
        StartCoroutine(WriteoutSentence(currentSentence.GetDescription(), question));
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
        currentDictator = Mathf.Clamp(i, 0, dictators.Length);

        dictators[currentDictator].PrepreDictator();
        dictatorName.SetText(dictators[currentDictator].GetDictatorName());

        dictatorRepresentation.sprite = dictators[currentDictator].GetDictatorImage();
        backgroundRepresentation.sprite = dictators[currentDictator].GetDictatorBackground();

        LoadDialog();
    }

    private void EndGame(MeetingResult result)
    {
        Debug.Log($"Game ended {lovePoints} !");
        gameplayPanel.SetActive(false);
        resultPanel.SetActive(true);
        switch (result)
        {
            case MeetingResult.Good:
                resultOutput.SetText("Great job soldier o7");
                break;
            case MeetingResult.Neutral:
                resultOutput.SetText("Meh, go back to playing games");
                break;
            case MeetingResult.Bad:
                resultOutput.SetText("Run, NOW!");
                break;
            default:
                resultOutput.SetText("How did we got here?");
                break;
        }
    }

    private IEnumerator WriteoutSentence(string sentence, TextMeshProUGUI output)
    {
        var waitTime = new WaitForSeconds(estimatedTimeToWriteOut/sentence.Length);
        output.text = "";
        for (int i=0; i<sentence.Length;i++) 
        {
            output.text += sentence[i];
            yield return waitTime;
            if (sentence[i] == ',' || sentence[i] == ',') yield return waitTime;
        }
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
public enum MeetingResult
{
    Good,
    Neutral,
    Bad
}