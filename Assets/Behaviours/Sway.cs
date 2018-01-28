using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sway : MonoBehaviour
{
    [SerializeField] float sway_radius;
    [SerializeField] float sway_speed;

    private Vector3 sway_point;
    private float next_sway_time;


    void Start()
    {
        
    }


    void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, sway_point, sway_speed * Time.deltaTime);

        if (Time.time >= next_sway_time)
        {
            GenerateSwayPoint();
        }
    }


    void GenerateSwayPoint()
    {
        next_sway_time = Time.time + Random.Range(0.25f, 0.75f);
        sway_point = Vector3.zero + (Random.insideUnitSphere * sway_radius);
    }




}
