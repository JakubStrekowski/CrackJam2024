using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] characterMusic;
    [SerializeField] private AudioSource audioSource;

    private AudioSource clickSoundSrc;
    
    [SerializeField] private GameObject successDictatorRepresentation; 
    [SerializeField] private Image dictatorRepresentation; 
    [SerializeField] private Image backgroundRepresentation; 

    [SerializeField] private GlobalSettings globalSettings;
    [SerializeField] private DictatorChan[] dictators;

    [SerializeField] private ParticleSystem happyParticles;
    [SerializeField] private ParticleSystem sadParticles;
    [SerializeField] private ParticleSystem clickParticles;

    [SerializeField] private BackToMenu backToMenuBtn;

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


    private Sequence _seq;
    private RectTransform _rect;
    [SerializeField] private Ease ease;
    [SerializeField] private Ease easeOut;

    [Header("Audio")] 
    [SerializeField] private AudioClip[] clicksSounds;
    
    private void Start()
    {
        _rect = dictatorRepresentation.gameObject.GetComponent<RectTransform>();
        clickSoundSrc = GetComponent<AudioSource>();
        
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
        winCap = lovePoints + dictators[currentDictator].GetTotalScore() - dictators[currentDictator].GetTotalScore() / 4;

        if (dictators[currentDictator].name == "StalineczkaDictator")
        {
            StartCoroutine(nameof(StalineczkaAnimController));
        }

        if (characterMusic.Length > 0)
        {
            audioSource.clip = characterMusic[currentDictator];
        }
        audioSource.Play();
    }

    private void Update()
    {
        Vector3 newPos = Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 poz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            poz.z = 0;
            clickParticles.transform.position = poz;
            clickParticles.Play();
        }
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
        if (clickSoundSrc != null)
        {
            int i = Random.Range(0, clicksSounds.Length);
            clickSoundSrc.PlayOneShot(clicksSounds[i]);
        }
        id = Mathf.Clamp(id, 0, currentSentence.GetAnswersCount()-1);

        int points = currentSentence.GetSentence(id).GetAnswetValue();
        Debug.Log(points);
        lovePoints += points;

        if (points > 0) 
        {
            if (_seq == null || !_seq.IsPlaying())
            {
                _seq = DOTween.Sequence();
                _seq.Append(_rect.DOMoveY(_rect.transform.position.y + 2, 0.5f).SetEase(ease));
                _seq.Append(_rect.DOMoveY(_rect.transform.position.y, 1).SetEase(easeOut));

                _seq.Play();
            }
            happyParticles.Play();
        }
        if (points < 0)
        {
            sadParticles.Play();
        }

        CalculateSpriteState(lovePoints);

        if (lovePoints >= winCap) EndGame(MeetingResult.Good);
        else if (lovePoints <= 0) EndGame(MeetingResult.Bad);
        LoadDialog();
    }

    private void CalculateSpriteState(int currentPoints)
    {
        if (dictators[currentDictator].name == "StalineczkaDictator")
        {
            if (currentPoints < 4)
            {
                dictators[currentDictator].stalineczkaAnimState = EStalineczkaAnimState.Angri;
            }
            else
            {
                dictators[currentDictator].stalineczkaAnimState = EStalineczkaAnimState.Neutral;
            }
        }
        else
        {
            float thresholdValue = dictators[currentDictator].GetTotalScore() /
                                   (float)dictators[currentDictator].GetSkinCount();

            int selectedIndex = Mathf.FloorToInt((float)currentPoints / thresholdValue);
            selectedIndex = Math.Clamp(selectedIndex, 0, dictators[currentDictator].GetSkinCount() - 1);

            dictatorRepresentation.sprite = dictators[currentDictator].skinStates[selectedIndex];
        }
        
    }

    private Coroutine _writeCoroutine;
    
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
        
        if (_writeCoroutine != null)
        {
            StopCoroutine(_writeCoroutine);
        }
        _writeCoroutine = StartCoroutine(WriteoutSentence(currentSentence.GetDescription(), question));
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

    private bool _isEnded;
    private void EndGame(MeetingResult result)
    {
        _isEnded = true;
        Debug.Log($"Game ended {lovePoints} !");
        gameplayPanel.SetActive(false);
        resultPanel.SetActive(true);
        backToMenuBtn.gameObject.SetActive(false);
        switch (result)
        {
            case MeetingResult.Good:
                SuccessAction();
                break;
            case MeetingResult.Neutral:
                if (lovePoints > 0.75f * winCap)
                {
                    SuccessAction();
                }
                else
                {
                    resultOutput.SetText("\"It's getting late, how about we meet another time? ;)\"");
                }
                break;
            case MeetingResult.Bad:
                resultOutput.SetText("Run, NOW!");
                dictatorRepresentation.gameObject.SetActive(false);
                successDictatorRepresentation.SetActive(true);
                successDictatorRepresentation.GetComponent<DictatorSuccess>()
                    .SetDictatorFailure(dictators[currentDictator]);

                if (dictators[currentDictator].backgroundAnimationAtEnd != null)
                {
                    if (dictators[currentDictator].backgroundAnimationAtEnd.Length > 0)
                    {
                        StartCoroutine(EndingBackgroundAnimation());
                    }
                }

                break;
            default:
                resultOutput.SetText("How did we got here?");
                break;
        }
    }

    private void SuccessAction()
    {
        resultOutput.SetText("\"I had a great time with you~! :3 How about we take it a bit further...? ;) Mmmm... <333\"");
        dictatorRepresentation.gameObject.SetActive(false);
        successDictatorRepresentation.SetActive(true);
        successDictatorRepresentation.GetComponent<DictatorSuccess>()
            .SetDictatorSuccess(dictators[currentDictator]);
    }

    private IEnumerator EndingBackgroundAnimation()
    {
        int i = 0;
        int l = dictators[currentDictator].backgroundAnimationAtEnd.Length;
        while (true)
        {
            backgroundRepresentation.sprite = dictators[currentDictator].backgroundAnimationAtEnd[i % l];
            i++;
            yield return new WaitForSeconds(0.25f);
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

    private IEnumerator StalineczkaAnimController()
    {
        while (!_isEnded)
        {
            if (dictators[currentDictator].stalineczkaAnimState == EStalineczkaAnimState.Neutral)
            {
                dictators[currentDictator].stalineczkaSpriteId = (dictators[currentDictator].stalineczkaSpriteId + 1) % dictators[currentDictator].stalineczkaNeutral.Length;
                
                dictatorRepresentation.sprite = dictators[currentDictator]
                    .stalineczkaNeutral[dictators[currentDictator].stalineczkaSpriteId];
            }
            if (dictators[currentDictator].stalineczkaAnimState == EStalineczkaAnimState.Angri)
            {
                dictators[currentDictator].stalineczkaSpriteId = (dictators[currentDictator].stalineczkaSpriteId + 1) % dictators[currentDictator].stalineczkaAngri.Length;
                
                dictatorRepresentation.sprite = dictators[currentDictator]
                    .stalineczkaAngri[dictators[currentDictator].stalineczkaSpriteId];
            }

            yield return new WaitForSeconds(0.25f);
        }
    }
}
public enum MeetingResult
{
    Good,
    Neutral,
    Bad
}