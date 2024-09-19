using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;
    public static AudioManager instance;
    void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }


        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.volume = s.volume;
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.mixer;
            s.source.loop = s.loop;



        }
        sounds[0].source.Play();

    }

    void Update()
    {
      
    }

}

[System.Serializable]
public class Sound
{

    public string name;
    [Range(0f, 1f)]
    public float volume;
    [Range(0f, 1f)]
    public float filter;
    public AudioClip clip;
    public AudioSource source;
    public AudioMixerGroup mixer;
    public bool loop;
}
