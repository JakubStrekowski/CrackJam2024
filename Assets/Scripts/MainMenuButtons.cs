using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    private const int GameplaySceneId = 1;

    public AudioMixer mixer;

    public void StartGame()
    {
        SceneManager.LoadScene(GameplaySceneId);
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    public void SetLevel (float sliderValue)
    {
        mixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20);
    }
    
}
