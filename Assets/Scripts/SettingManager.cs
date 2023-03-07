using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public void setVolumeMusic(float volumeM)
    {
        audioMixer.SetFloat("Music", volumeM);
    }
    public void setVolumeSound(float volumeS)
    {
        audioMixer.SetFloat("Sound", volumeS);
    }
}
