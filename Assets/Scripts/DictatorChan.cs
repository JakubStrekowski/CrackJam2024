using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Dictator")]
public class DictatorChan : ScriptableObject
{
    [SerializeField] private string dictatorName;
    [SerializeField] private string dictatorDescription;
    [Space]
    [SerializeField] private Sprite image;
    [SerializeField] private Sprite background;
    [Space]
    [SerializeField] private Sentence[] sentencesReference;

    public Sprite[] skinStates;

    private List<Sentence> sentencesBuffer = new List<Sentence>();

    public void PrepreDictator()
    {
        sentencesBuffer.Clear();
        sentencesBuffer = new List<Sentence>(sentencesReference);

    }

    public int GetTotalScore()
    {
        return sentencesReference.Length;
    }

    public int GetSkinCount()
    {
        return skinStates.Length;
    }
    
    public bool IsMoreSentences()
    {
        return sentencesBuffer.Count > 0;
    }
    public Sentence GetNextSentence()
    {
        if (IsMoreSentences())
        {
            int id = Random.Range(0, sentencesBuffer.Count);

            Sentence sent = sentencesBuffer[id];
            sentencesBuffer.RemoveAt(id);

            return sent;
        }
        return null;
    }
    public string GetDictatorName()
    {
        return dictatorName;
    }
    public string GetDictatorDescription()
    {
        return dictatorDescription;
    }
    public Sprite GetDictatorImage()
    {
        return image;
    }
    public Sprite GetDictatorBackground() 
    {
        return background;
    }
}
