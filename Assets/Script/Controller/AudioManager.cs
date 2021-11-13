using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : SingletonMonoBehavier<AudioManager>
{
    public Sound[] sounds;
    //public AudioMixerGroup mixer;
    private void Awake()
    {
        foreach(Sound sound in sounds)
        {
            sound.source = gameObject.GetComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            //sound.source.pitch = sound.pitch;
            //sound.source.outputAudioMixerGroup = mixer;
        }
        
    }
    private void Start()
    {
        //DontDestroyOnLoad(this);
    }
    public void Play(string name)
    {
        if(!GameRes.SoundOn) return;
        Sound sound = Array.Find(sounds, s => s.name == name);
        sound.source.PlayOneShot(sound.clip);
    }
    public void Stop(string name)
    {
        Sound sound = Array.Find(sounds, s => s.name == name);
        sound.source.Stop();
    }
}
public class MusicName
{
    public const string Punch = "Punch";
}
