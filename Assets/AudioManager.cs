using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioMixer mixer;
    public Slider VolSlide;

    void Start()
    {
        float db;
        mixer.GetFloat("Volume", out db);
        VolSlide.value = (db +80)/80;
    }
public void Mute(bool Diam)
{
    if(Diam)
    {
        AudioListener.volume=0;
    }
    else 
    {
        AudioListener.volume=1;
    }
}

public void Mixer(float value)
{
    value = value*80 - 80;

    mixer.SetFloat("Volume", value);
}


}
