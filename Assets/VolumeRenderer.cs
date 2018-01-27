using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeRenderer : MonoBehaviour
{
    [SerializeField] Vector3 size;
    //[SerializeField] private uint octaves = 6;
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
          if (Input.GetKey(KeyCode.A))
            UpdateClouds();

          UpdateVolumeRenderer();
    }


    void GenerateClouds()
    {
        Color[] pixels = new Color[(int)(size.x * size.y * size.z)];
        for (int y = 0; y < size.y; ++y)
        {
            for (int x = 0; x < size.x; ++x)
            {
                for (int z = 0; z < size.z; ++z)
                {
                    float colour_value = Noise.Perlin3D(new Vector3(x, y, z), frequency);

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


    float PerlinNoise3D(float _x, float _y, float _z)
    {
        float ab = Mathf.PerlinNoise(_x, _y);
        float bc = Mathf.PerlinNoise(_y, _z);
        float ac = Mathf.PerlinNoise(_x, _z);

        float ba = Mathf.PerlinNoise(_y, _x);
        float cb = Mathf.PerlinNoise(_z, _y);
        float ca = Mathf.PerlinNoise(_z, _x);

        float abc = ab + bc + ac + ba + cb + ca;
        return abc / 6;
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
