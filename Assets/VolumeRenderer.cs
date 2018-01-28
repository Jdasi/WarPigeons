using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class VolumeRenderer : MonoBehaviour
{
    [SerializeField] Vector3 size;
    [SerializeField] private float scale = 5f;
    [SerializeField] private float frequency = 0.6f;
    [SerializeField] public int seed = 0;
    [SerializeField] public float contrast_low = 0f; 
    [SerializeField] public float contrast_high = 1f;
    [SerializeField] public float brightness_offset = 0f;
  

    private Texture3D texture; 
    private Renderer renderer;


    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        renderer = GetComponent<Renderer>();

        texture = new Texture3D(Mathf.NextPowerOfTwo((int) size.x), Mathf.NextPowerOfTwo((int)size.y),
            Mathf.NextPowerOfTwo((int)size.z), TextureFormat.ARGB32, true);

        UpdateClouds();
    }


    void UpdateClouds()
    {
        GenerateClouds();
        UpdateVolumeRenderer();
    }


    void Update ()
    {     
          if (Input.GetKey(KeyCode.U))
            UpdateClouds();

          UpdateVolumeRenderer();
    }


    private void GenerateClouds()
    {
        Color[] pixels = new Color[(int)(size.x * size.y * size.z)];
        for (int y = 0; y < size.y; ++y)
        {
            for (int x = 0; x < size.x; ++x)
            {
                for (int z = 0; z < size.z; ++z)
                {
                    float colour_value = Noise.Perlin3D(new Vector3(x, y, z) + transform.position, frequency);

                    colour_value = Mathf.Clamp(colour_value, contrast_low,
                        contrast_high + contrast_low) - contrast_low;
                    colour_value = Mathf.Clamp(colour_value, 0f, 1f);
                    colour_value += brightness_offset;

                    pixels[(int)(x + y * size.x + z * size.x * size.y)] =
                        new Color(colour_value, colour_value, colour_value, colour_value);
                }
            }
        }

        texture.SetPixels(pixels);
        texture.Apply();
    }


    void UpdateVolumeRenderer()
    {
        transform.rotation = Quaternion.identity;

        if (texture == null)//exit if there is no volume to render
            return;
        renderer.material.SetVector("_translation", transform.localPosition);
        renderer.material.SetVector("_scale", transform.localScale);
        renderer.material.SetTexture("_density", texture);
        renderer.material.SetVector("_size", new Vector3(texture.width, texture.height, texture.depth));
    }
}
