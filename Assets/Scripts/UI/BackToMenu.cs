using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenu : MonoBehaviour
{
    private const int MainMenuId = 0;
    
    public void GotoMainMenu()
    {
        SceneManager.LoadScene(MainMenuId);
    }
}
