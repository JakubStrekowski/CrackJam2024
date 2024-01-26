using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SentenceAnswer
{
    [SerializeField] private string answer;
    [SerializeField] private int points;

    public string GetAnswerText()
    {
        return answer;
    }
    public int GetAnswetValue()
    {
        return points;
    }
}