using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OptionsData
{
    public float musicVolume;
    public float effectVolume;

    public OptionsData (AudioManager audioManager)
    {
        musicVolume = audioManager.musicVolume;
        effectVolume = audioManager.effectVolume;
    }
}
