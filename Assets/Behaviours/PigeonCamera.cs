﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigeonCamera : MonoBehaviour
{
    [SerializeField] float follow_lerp_speed = 20;
    [SerializeField] float fov_lerp_speed = 5;
    [SerializeField] float look_at_speed = 5;
    [SerializeField] Vector3 follow_offset;

    [Header("References")]
    [SerializeField] Camera cam;

    private float target_fov;
    private Pigeon pigeon;
    private float target_follow_speed;


    public void SetFollowSpeed(float _speed)
    {
        target_follow_speed = _speed;
    }


    public void SetFOV(float _fov)
    {
        target_fov = _fov;
    }


    void Start()
    {
        pigeon = GameManager.scene.pigeon;

        target_follow_speed = follow_lerp_speed;
        target_fov = cam.fieldOfView;
    }

    
    void Update()
    {
        follow_lerp_speed = Mathf.Lerp(follow_lerp_speed, target_follow_speed, 15 * Time.deltaTime);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, target_fov, fov_lerp_speed * Time.deltaTime);
    }


    void FixedUpdate()
    {
        Follow();
    }


    void Follow()
    {
        transform.position = Vector3.Lerp(transform.position, pigeon.cam_follow_target.position + follow_offset, follow_lerp_speed * Time.deltaTime);
        transform.LookAt(pigeon.cam_lookat_target.position);

        //Quaternion rot = Quaternion.LookRotation(look_target.position - transform.position);
        //transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, look_at_speed * Time.fixedDeltaTime);
    }

}
