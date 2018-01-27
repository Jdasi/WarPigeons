using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigeonCamera : MonoBehaviour
{
    [SerializeField] float follow_lerp_speed = 20;
    [SerializeField] float fov_lerp_speed = 5;
    [SerializeField] Vector3 follow_offset;

    [Space]
    [SerializeField] Transform follow_target;
    [SerializeField] Transform look_target;

    [Header("References")]
    [SerializeField] Pigeon pigeon;
    [SerializeField] Camera cam;

    private float target_fov;


    public void SetFOV(float _fov)
    {
        target_fov = _fov;
    }


    void Start()
    {
        target_fov = cam.fieldOfView;
    }

    
    void Update()
    {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, target_fov, fov_lerp_speed * Time.deltaTime);
    }


    void FixedUpdate()
    {
        Follow();
    }


    void Follow()
    {
        transform.position = Vector3.Lerp(transform.position, follow_target.position + follow_offset, follow_lerp_speed * Time.deltaTime);
        transform.LookAt(look_target.position);
    }


    void OnDrawGizmos()
    {
        if (follow_target == null || look_target == null)
            return;

        Gizmos.DrawRay(follow_target.position, look_target.position - follow_target.position);
    }
}
