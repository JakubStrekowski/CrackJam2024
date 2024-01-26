using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Dictator")]
public class DictatorChan : ScriptableObject
{
    [SerializeField] private string dictatorName;
    [SerializeField] private string dictatorDescription;

    [SerializeField] private Sentence[] sentencesReference;

    private List<Sentence> sentencesBuffer = new List<Sentence>();

    public void PrepreDictator()
    {
        sentencesBuffer.Clear();
        sentencesBuffer = new List<Sentence>(sentencesReference);

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
}
