using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    public static AudioManager instance;
    public static AudioManager Instance { get { return instance; } }

    //crear un valor global al volumen y a la musica para poder acceder a él rapidamente?
    private float volumeValue;
    private float musicValue;

    //En el código donde queremos que suene:
    //FindObjectOfType<AudioManager>().Play("");


    void Awake()
    {
        volumeValue = 0.5f;
        musicValue = 0.5f;
        Debug.LogWarning("Awake");
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            if (s.musica)
                s.source.volume = musicValue * s.porcentaje * s.volume;
            else
                s.source.volume = volumeValue * s.porcentaje * s.volume;

            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    void Start()
    {

        Debug.LogWarning("Start del audiomanager");

    }

    public void Play(String name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found! MIREN EL NOMBRE O ALGO BRO");
            return;
        }
        s.source.Play();
    }
    public void Stop(String name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found! MIREN EL NOMBRE O ALGO BRO");
            return;
        }
        s.source.Stop();
    }

    public void changeVolumeSFX(float vol)
    {
        Debug.LogWarning("0000");
        if (vol >= 0 && vol <= 1)
        {
            Debug.LogWarning("11111");
            foreach (Sound s in sounds)
            {
                if (!s.musica)
                {
                    s.source.volume = s.porcentaje * vol;
                    Debug.LogWarning("Sound: " + s.name + " vol: " + s.source.volume);
                }
                else
                    Debug.LogWarning("no tiene sonidos");
            }
            setVolumeValue(vol);
        }

    }
    public void changeMusicValue(float vol)
    {
        if (vol >= 0 && vol <= 1)
        {
            foreach (Sound s in sounds)
            {
                if (s.musica)
                    s.source.volume = s.porcentaje * vol;
            }
            setVolumeValue(vol);
        }

    }
    public void setVolumeValue(float n)
    {
        if (n >= 0 && n <= 1)
        {
            volumeValue = n;
        }
    }
    public void setMusicValue(float n)
    {
        if (n >= 0 && n <= 1)
        {
            musicValue = n;
        }
    }
    public float GetFloatVolume()
    {
        return volumeValue;
    }
    public float GetFloatMusic()
    {
        return musicValue;
    }
}
