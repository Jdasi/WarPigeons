using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sinner : MonoBehaviour
{
    [SerializeField] float scale_speed = 1;
    [SerializeField] float sin_factor = 1;

    private Vector3 start_scale;


    void Start()
    {
        start_scale = transform.localScale;
    }


    void Update()
    {
        float sin = Mathf.Sin(scale_speed * Time.time) * sin_factor;
        transform.localScale = start_scale + (start_scale * sin);
    }

}
