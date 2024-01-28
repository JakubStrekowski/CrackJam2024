using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] private GameObject adultPromt;
    [SerializeField] private Button adultConsent;
    [SerializeField] private Slider volumeSlider;
    
    private const int GameplaySceneId = 1;

    public AudioMixer mixer;

    [ContextMenu("ClearPrefs")]
    public void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
    private void Start()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        //bool b = PlayerPrefs.GetInt("Agreed") >= (int)(5);
        adultPromt.gameObject.SetActive(true);
    }

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

    public void SetLevel (float value)
    {
        float sliderValue = volumeSlider.value;
        mixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
    }

    public void SetConsent(bool value)
    {
        adultConsent.interactable = value;
    }
    
}
