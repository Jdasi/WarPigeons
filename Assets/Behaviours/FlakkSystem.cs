using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlakkSystem : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] float min_shot_delay;
    [SerializeField] float max_shot_delay;

    [Space]
    [SerializeField] Vector3 aiming_extents;
    [SerializeField] float shot_damage_radius;

    [Space]
    [SerializeField] float engage_height;
    [SerializeField] float engage_delay;

    [Header("References")]
    [SerializeField] GameObject smoke_particle;

    [Header("Debug")]
    [SerializeField] GameObject target;

    private float next_shot_time;
    private float engage_timer;
    private bool can_engage { get { return engage_timer >= engage_delay; } }


    void Start()
    {
        next_shot_time = Time.time;
    }


    void Update()
    {
        HandleShooting();
    }


    void HandleShooting()
    {
        if (target == null || Time.time < next_shot_time)
            return;

        if (target.transform.position.y < engage_height)
        {
            engage_timer = 0;
            return;
        }

        if (!can_engage)
        {
            engage_timer += Time.deltaTime;
            
            if (!can_engage)
                return;
        }

        Shoot();
    }


    void Shoot()
    {
        next_shot_time = Time.time + Random.Range(min_shot_delay, max_shot_delay);

        float x = Random.Range(-aiming_extents.x, aiming_extents.x);
        float y = Random.Range(-aiming_extents.y, aiming_extents.y);
        float z = Random.Range(-aiming_extents.z, aiming_extents.z);

        Vector3 shoot_pos = target.transform.position + new Vector3(x, y, z);
        Instantiate(smoke_particle, shoot_pos, Quaternion.identity);

        var hits = Physics.OverlapSphere(shoot_pos, shot_damage_radius);
        foreach (var hit in hits)
        {
            // if bird, damage
        }
    }


    void OnDrawGizmos()
    {
        Gizmos.color = can_engage ? Color.red : Color.green;
        Gizmos.DrawWireCube(target == null ? transform.position : target.transform.position, aiming_extents * 2);
    }

}
