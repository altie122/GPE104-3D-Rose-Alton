using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class SettingsMenu : MonoBehaviour
{
    
    private InputSystem_Actions inputSystem;
    
    private PanelRenderer panelRenderer;
    

    private Slider masterAudioSlider;
    
    private Slider musicAudioSlider;

    private Slider sfxAudioSlider;
    
    public AudioMixer audioMixer;

    private VisualElement settingsMenuRoot;
    
    public PauseMenu pauseMenu;
    
    private struct AudioSliderSettings
    {
        public float value;
        public float lowValue;
        public float highValue;
    }
    
    public void OnEnable()
    {
        GetComponent<PanelRenderer>().RegisterUIReloadCallback(OnUIReload);
    }

    public void OnDisable()
    {
        GetComponent<PanelRenderer>().UnregisterUIReloadCallback(OnUIReload);
        
        masterAudioSlider?.UnregisterAllRemovableCallbacks();
        musicAudioSlider?.UnregisterAllRemovableCallbacks();
        sfxAudioSlider?.UnregisterAllRemovableCallbacks();

        inputSystem?.Disable();
        inputSystem?.Dispose();
        inputSystem = null;
    }

    private void OnUIReload(
        PanelRenderer renderer,
        VisualElement rootElement,
        int version
    )
    {
        
        inputSystem = new InputSystem_Actions();
        
        inputSystem.Enable();
        
        masterAudioSlider = rootElement.Q<Slider>("MasterAudio");
        
        musicAudioSlider = rootElement.Q<Slider>("MusicAudio");
        
        sfxAudioSlider = rootElement.Q<Slider>("SFXAudio");
        
        LoadMasterAudioSlider();
        LoadMusicAudioSlider();
        LoadSFXAudioSlider();

        masterAudioSlider.RegisterValueChangedCallback(OnMasterAudioChanged);

        musicAudioSlider.RegisterValueChangedCallback(OnMusicAudioChanged);

        sfxAudioSlider.RegisterValueChangedCallback(OnSFXAudioChanged);
        
        
        settingsMenuRoot = rootElement;
        
        settingsMenuRoot.style.display = DisplayStyle.None;
    }
    public void ShowSettings()
    {
        inputSystem.Player.Pause.performed += HideSettings;
        settingsMenuRoot.style.display = DisplayStyle.Flex;
    }
    
    public void HideSettings(InputAction.CallbackContext context)
    {
        inputSystem.Player.Pause.performed -= HideSettings;
        settingsMenuRoot.style.display = DisplayStyle.None;
        pauseMenu.ShowPauseMenu();
    }
    
    private void SaveAudioSlider(
        string mixerParameter,
        Slider slider,
        float value
    )
    {
        var settings = new AudioSliderSettings
        {
            value = value,
            lowValue = slider.lowValue,
            highValue = slider.highValue
        };

        PlayerPrefs.SetString(
            $"{mixerParameter}Slider",
            JsonUtility.ToJson(settings)
        );

        GameManager.instance.SetAudioVolume(
            mixerParameter,
            slider.lowValue,
            slider.highValue,
            value
        );
    }
    
    private void OnMasterAudioChanged(ChangeEvent<float> evt)
    {
        SaveAudioSlider("MasterVolume", masterAudioSlider, evt.newValue);
    }

    private void OnMusicAudioChanged(ChangeEvent<float> evt)
    {
        SaveAudioSlider("MusicVolume", musicAudioSlider, evt.newValue);
    }

    private void OnSFXAudioChanged(ChangeEvent<float> evt)
    {
        SaveAudioSlider("SFXVolume", sfxAudioSlider, evt.newValue);
    }
    
    public void LoadMasterAudioSlider()
    {
        const string key = "MasterVolumeSlider";

        if (!PlayerPrefs.HasKey(key))
        {
            return;
        }

        var settings = JsonUtility.FromJson<AudioSliderSettings>(
            PlayerPrefs.GetString(key)
        );

        masterAudioSlider.lowValue = settings.lowValue;
        masterAudioSlider.highValue = settings.highValue;
        masterAudioSlider.SetValueWithoutNotify(settings.value);

        GameManager.instance.SetAudioVolume(
            "MasterVolume",
            settings.lowValue,
            settings.highValue,
            settings.value
        );
    }
    
    public void LoadMusicAudioSlider()
    {
        const string key = "MusicVolumeSlider";

        if (!PlayerPrefs.HasKey(key))
        {
            return;
        }

        var settings = JsonUtility.FromJson<AudioSliderSettings>(
            PlayerPrefs.GetString(key)
        );

        musicAudioSlider.lowValue = settings.lowValue;
        musicAudioSlider.highValue = settings.highValue;
        musicAudioSlider.SetValueWithoutNotify(settings.value);

        GameManager.instance.SetAudioVolume(
            "MusicVolume",
            settings.lowValue,
            settings.highValue,
            settings.value
        );
    }
    
    public void LoadSFXAudioSlider()
    {
        const string key = "SFXVolumeSlider";

        if (!PlayerPrefs.HasKey(key))
        {
            return;
        }

        var settings = JsonUtility.FromJson<AudioSliderSettings>(
            PlayerPrefs.GetString(key)
        );

        sfxAudioSlider.lowValue = settings.lowValue;
        sfxAudioSlider.highValue = settings.highValue;
        sfxAudioSlider.SetValueWithoutNotify(settings.value);

        GameManager.instance.SetAudioVolume(
            "SFXVolume",
            settings.lowValue,
            settings.highValue,
            settings.value
        );
    }
}
