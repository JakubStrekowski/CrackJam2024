using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Sentence")]
public class Sentence : ScriptableObject
{
    [SerializeField] private string description;
    [SerializeField] private SentenceAnswer[] answers;

    public string GetDescription()
    {
        return description;
    }

    public SentenceAnswer[] GetAnswers()
    {
        return answers;
    }

    public SentenceAnswer GetSentence(int id)
    {
        if (id < answers.Length)
        {
            return answers[id];
        }
        Debug.LogError("Answer aout of bound!");
        return null;
    }
    public int GetAnswersCount()
    {
        return answers.Length;
    }
}

