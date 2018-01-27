using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AudioManager : MonoBehaviour
{
    public static AudioSettings settings { get { return instance.settings_; } }

    [SerializeField] AudioSettings settings_;

    private static AudioManager instance;

    private AudioSource music_source;
    private AudioSource sfx_source;
    private AudioSource sfx_unscaled_source;

    private AudioClip last_clip_played;


    public static void PlayOneShot(string _clip_name)
    {
        PlayOneShot(instance.GetAudioClip(_clip_name));
    }


    public static void PlayOneShot(AudioClip _clip)
    {
        if (instance.last_clip_played == _clip)
            return;

        instance.last_clip_played = _clip;

        if (_clip != null)
            instance.sfx_source.PlayOneShot(_clip);
    }


    public static void PlayOneShotUnscaled(string _clip_name)
    {
        PlayOneShotUnscaled(instance.GetAudioClip(_clip_name));
    }


    public static void PlayOneShotUnscaled(AudioClip _clip)
    {
        if (instance.last_clip_played == _clip)
            return;

        instance.last_clip_played = _clip;

        if (_clip != null)
            instance.sfx_unscaled_source.PlayOneShot(_clip);
    }


    public static void StopAllSFX()
    {
        instance.sfx_source.Stop();
    }


    public AudioClip GetAudioClip(string _clip_name)
    {
        return settings.sfx_clips.Find(elem => elem != null &&
            elem.name.Substring(0) == _clip_name);
    }


    public void PlayRandomMusic()
    {
        if (settings.music_clips.Count == 0)
            return;

        music_source.Stop();

        int index = Random.Range(0, settings.music_clips.Count);

        music_source.clip = settings.music_clips[index];
        music_source.loop = true;

        music_source.Play();
    }


    void Awake()
    {
        if (instance == null)
        {
            InitSingleton();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    void InitSingleton()
    {
        instance = this;

        GameObject audio_parent = new GameObject("Audio");
        audio_parent.transform.SetParent(this.transform);

        music_source = audio_parent.AddComponent<AudioSource>();
        sfx_source = audio_parent.AddComponent<AudioSource>();
        sfx_unscaled_source = audio_parent.AddComponent<AudioSource>();

        music_source.volume = settings.music_volume;
        sfx_source.volume = settings.sfx_volume;
        sfx_unscaled_source.volume = settings.sfx_volume;

        PlayRandomMusic();
    }


    void Update()
    {
        sfx_source.pitch = Time.timeScale;
    }


    void LateUpdate()
    {
        last_clip_played = null;
    }

}
