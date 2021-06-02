﻿using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    [Range(0f, 1f)]
    public float musicVolume = 1f;

    [Range(0f, 1f)]
    public float effectVolume = 1f;

    public static AudioManager instance;

    public Slider MSlider, ESlider;

    void Awake()
    {
        transform.parent = null;

        if (instance == null)
        {
            instance = this;
        }

        else
        {
            Destroy(gameObject);
            return;
        }
        //DontDestroyOnLoad(gameObject);
/*
        MSlider = transform.Find("Slider_Music").GetComponent<Slider>();
        ESlider = transform.Find("Slider_Effect").GetComponent<Slider>();*/

        if (MSlider != null && ESlider != null)
        {
            MSlider.value = 1f;
            ESlider.value = 1f;
        }
        else Debug.Log("Slider problems");

        print(MSlider);
        print(ESlider);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;
        }



    }
    private void Start()
    {
        OptionsData data = SaveSystem.LoadOptions();
        if(data != null)
        {
            musicVolume = data.musicVolume;
            effectVolume = data.effectVolume;
            if (MSlider != null && ESlider != null)
            {
                MSlider.value = musicVolume;
                ESlider.value = effectVolume;
            }
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound : " + name + "not found!");
            return;
        }

        if (s.type == Sound.Type.Music)
            s.source.volume = s.volume * musicVolume;
        else
            print(s);
            s.source.volume = s.volume * effectVolume;

        s.source.Play();
    }
    public void StopPlaying(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        s.source.volume = s.volume;

        s.source.Stop();
    }

    public void StopPlayAll()
    {
        for (int i =0; i< sounds.Length; i++)
        {
            StopPlaying(sounds[i].name);
        }
    }

    public void ApplyChanges()
    {
        print("ùlk,poezgrk,loùegrts,knlmjoegrzt");
        print("volume music: " + musicVolume);
        print("volume effet: " + effectVolume);
        musicVolume = MSlider.value;
        effectVolume = ESlider.value;
        for (int i = 0; i< sounds.Length; i++)
        {
            if (sounds[i].type == Sound.Type.Music)
                sounds[i].source.volume = sounds[i].volume * musicVolume;
            else
                sounds[i].source.volume = sounds[i].volume * effectVolume;
        }
        SaveSystem.SaveOptions(this);
    }

}

