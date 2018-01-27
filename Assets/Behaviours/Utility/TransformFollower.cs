using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class TransformFollower : MonoBehaviour
{
    [SerializeField] bool fixed_update;
    [SerializeField] float lerp_speed;
    [SerializeField] Vector3 follow_offset;

    [Space]
    [SerializeField] Transform follow_target;
    [SerializeField] Transform look_target;

    
    void Update()
    {
        if (fixed_update)
            return;

        Follow();
    }


    void FixedUpdate()
    {
        if (!fixed_update)
            return;

        Follow();
    }


    void Follow()
    {
        transform.position = Vector3.Lerp(transform.position, follow_target.position + follow_offset, lerp_speed * Time.deltaTime);
        transform.LookAt(look_target.position);
    }


    void OnDrawGizmos()
    {
        if (follow_target == null || look_target == null)
            return;

        Gizmos.DrawRay(follow_target.position, look_target.position - follow_target.position);
    }

}
