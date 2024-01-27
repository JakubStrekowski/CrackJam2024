using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "ScriptableObjects/GlobalSettings")]
public class GlobalSettings : ScriptableObject
{
    public int choosenWaifu;

    public void ChooseWaifu(int decision)
    {
        choosenWaifu = decision;
        SceneManager.LoadScene(1);
    }
}
