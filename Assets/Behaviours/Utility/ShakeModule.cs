using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeModule : MonoBehaviour
{
    public bool paused;

    [SerializeField] AnimationCurve decay_rate;

    private bool shaking;
    private float shake_strength;
    private float shake_duration;
    private float shake_time;
    private Vector3 original_local_pos;

    
    public void Init(AnimationCurve _decay_rate)
    {
        decay_rate = _decay_rate;
    }


    public void Shake(float _strength, float _duration)
    {
        shake_time = 0.0f;

        shake_strength = _strength;
        shake_duration = _duration;

        if (!shaking)
        {
            original_local_pos = this.transform.localPosition;
            shaking = true;
        }
    }


    void Start()
    {

    }


    void Update()
    {
        if (paused)
            return;

        if (shaking)
        {
            shake_time += Time.deltaTime;

            HandleShake();
        }
    }


    void HandleShake()
    {
        if (shake_time < shake_duration)
        {
            this.transform.localPosition = original_local_pos +
                ((Random.insideUnitSphere * shake_strength * Time.timeScale) *
                decay_rate.Evaluate(shake_time / shake_duration));
        }
        else
        {
            shaking = false;
            this.transform.localPosition = original_local_pos;
        }
    }

}
