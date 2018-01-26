using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Attach to a GameObject with an Image, SpriteRenderer, or Text component.
public class FadableGraphic : MonoBehaviour
{
    public ListenerModule listener_module = new ListenerModule();

    [SerializeField] bool pulse_mode;
    [SerializeField] float pulse_speed;
    [SerializeField] float pulse_low;
    [SerializeField] float pulse_high = 1;

    private Image image;
    private SpriteRenderer sprite;
    private Text text;

    private bool fading;
    private float fade_progress;
    private float fade_duration;

    private Color starting_color;
    private Color target_color;


    public void SetBaseColor(Color _color)
    {
        if (image != null)
            image.color = _color;

        if (sprite != null)
            sprite.color = _color;

        if (text != null)
            text.color = _color;
    }


    public void FadeColor(Color _from, Color _to, float _t)
    {
        starting_color = _from;
        target_color = _to;

        fade_duration = _t;

        StartFade();
    }


    public void FadeColor(Color _to, float _t)
    {
        FadeColor(GetGraphicColor(), _to, _t);
    }


    public void FadeAlpha(float _from, float _to, float _t)
    {
        Color from = GetGraphicColor();
        from.a = _from;

        Color to = from;
        to.a = _to;

        FadeColor(from, to, _t);
    }


    public void FadeAlpha(float _to, float _t)
    {
        FadeAlpha(GetGraphicColor().a, _to, _t);
    }


    public void FadeIn(float _t)
    {
        FadeAlpha(0, 1, _t);
    }


    public void FadeOut(float _t)
    {
        FadeAlpha(1, 0, _t);
    }


    public void FadeFrom(Color _color, float _t)
    {
        Color from = _color;
        Color to = GetGraphicColor();

        FadeColor(from, to, _t);
    }


    public void FadeTo(Color _color, float _t)
    {
        Color from = GetGraphicColor();
        Color to = _color;

        FadeColor(from, to, _t);
    }


    public void CancelFade()
    {
        fading = false;
    }


    public void Init()
    {
        // Detect what sort of graphic we are.
        image = GetComponent<Image>();
        sprite = GetComponent<SpriteRenderer>();
        text = GetComponent<Text>();
    }


    public void Init(GameObject _listener)
    {
        listener_module.AddListener(_listener);
        Init();
    }


    void Awake()
    {
        Init();
    }


    void Update()
    {
        if (fading)
        {
            HandleFade();
        }
        else if (pulse_mode)
        {
            HandlePulse();
        }
    }


    void HandleFade()
    {
        fade_progress += Time.deltaTime;
        Color color = Color.Lerp(starting_color, target_color, fade_progress / fade_duration);

        if (image)
        {
            image.color = color;
        }
        else if (sprite)
        {
            sprite.color = color;
        }
        else if (text)
        {
            text.color = color;
        }

        // Determine if fade is complete.
        if (color == target_color)
        {
            StopFade();
            
            listener_module.NotifyListeners("FadableGraphicDone");
        }
    }


    void HandlePulse()
    {
        float alpha_pulse = (1 + pulse_low) + Mathf.Sin(Time.time * pulse_speed);
        if (alpha_pulse > pulse_high)
            alpha_pulse = pulse_high;

        Color color = new Color();

        if (image)
        {
            color = image.color;
        }
        else if (sprite)
        {
            color = sprite.color;
        }
        else if (text)
        {
            color = text.color;
        }

        color.a = alpha_pulse;

        if (image)
        {
            image.color = color;
        }
        else if (sprite)
        {
            sprite.color = color;
        }
        else if (text)
        {
            text.color = color;
        }
    }


    void StartFade()
    {
        if (image)
        {
            image.color = starting_color;
        }
        else if (sprite)
        {
            sprite.color = starting_color;
        }
        else if (text)
        {
            text.color = starting_color;
        }

        fade_progress = 0;
        fading = true;
    }

    
    void StopFade()
    {
        fading = false;
        fade_progress = 0;
    }


    Color GetGraphicColor()
    {
        if (image)
        {
            return image.color;
        }
        else if (sprite)
        {
            return sprite.color;
        }
        else
        {
            return text.color;
        }
    }

}
