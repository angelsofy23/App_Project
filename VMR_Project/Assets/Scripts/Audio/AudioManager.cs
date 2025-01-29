using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    // Arrays de música e de efeitos sonoros
    public Sounds[] music, SFX;
    public AudioSource musicSource, SFXSource;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //Método para tocar música
    public void PlayMusic(string soundName)
    {
        Sounds s = Array.Find(music, x => x.soundName == soundName);

        if (s == null)
        {
            Debug.Log("Som não encontrado!");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    //Método para tocar efeitos sonoros
    public void PlaySoundEffects(string soundName)
    {
        Sounds s = Array.Find(SFX, x => x.soundName == soundName);

        if (s == null)
        {
            Debug.Log("Som não encontrado!");
        }
        else
        {
            SFXSource.PlayOneShot(s.clip);
        }
    }

    //Mutar música
    public void MuteMusic()
    {
        musicSource.mute = !musicSource.mute;
    }

    //Mutar efeitos sonoros
    public void MuteSFX()
    {
        SFXSource.mute = !SFXSource.mute;
    }

    //Método para ajustar o volume da música com base no valor do slider
    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    //Método para ajustar o volume dos efeitos sonoros com base no valor do slider
    public void SFXVolume(float volume)
    {
        SFXSource.volume = volume;
    }

    public void Start()
    {
        PlayMusic("Music");
    }
}

