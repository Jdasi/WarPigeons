using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadableSource : MonoBehaviour
{
    [SerializeField] AudioClip clip;
    [SerializeField] bool play_on_awake = true;
    [SerializeField] bool looping = false;
    [Range(0, 1)][SerializeField] float volume = 1;
    [Range(0, 1)][SerializeField] float pitch = 1;

    private AudioSource audio_source;

    private float fade_progress;
    private float fade_duration;
    private bool fading { get { return fade_progress < 1; } }

    private float starting_volume;
    private float target_volume;


    public void FadeVolume(float _target_volume, float _duration)
    {
        fade_progress = 0;
        fade_duration = _duration;

        starting_volume = volume;
        target_volume = _target_volume;
    }


    void Awake()
    {
        audio_source = gameObject.AddComponent<AudioSource>();
        audio_source.clip = clip;
        audio_source.playOnAwake = play_on_awake;
        audio_source.loop = looping;

        if (play_on_awake)
        {
            audio_source.Play();
        }
    }

    
    void Update()
    {
        audio_source.volume = volume;
        audio_source.pitch = pitch;

        if (fading)
        {
            Debug.Log("fading");
            HandleFade();
        }
    }


    void HandleFade()
    {
        fade_progress += Time.deltaTime;
        volume = Mathf.Lerp(starting_volume, target_volume, fade_progress / fade_duration);

        if (!fading)
        {
            fade_progress = 1;
            target_volume = 0;
        }
    }

}
