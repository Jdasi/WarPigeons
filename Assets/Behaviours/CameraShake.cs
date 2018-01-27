using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] AnimationCurve decay_rate;

    private static CameraShake instance;
    private ShakeModule shake_module;


    public static void Shake(float _strength, float _duration)
    {
        instance.shake_module.Shake(_strength, _duration);
    }


    public static void Pause()
    {
        instance.shake_module.paused = true;
    }


    public static void Resume()
    {
        instance.shake_module.paused = false;
    }


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            shake_module = this.gameObject.AddComponent<ShakeModule>();
            shake_module.Init(decay_rate);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

}
