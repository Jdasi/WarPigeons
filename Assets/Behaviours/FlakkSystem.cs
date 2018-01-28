using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlakkSystem : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] float min_shot_delay = 0.25f;
    [SerializeField] float max_shot_delay = 1.5f;

    [Space]
    [SerializeField] Vector3 aiming_extents;
    [SerializeField] float shot_damage_radius = 15;
    [SerializeField] float shot_damage = 45;

    [Space]
    [SerializeField] float engage_height = 30;
    [SerializeField] float engage_delay = 1;

    [Space]
    [SerializeField] float min_shake_strength = 0.33f;
    [SerializeField] float max_shake_strength = 0.5f;
    [SerializeField] float min_shake_duration = 0.33f;
    [SerializeField] float max_shake_duration = 0.5f;

    [Space]
    [SerializeField] List<AudioClip> flakk_clips;

    [Header("References")]
    [SerializeField] GameObject smoke_particle;

    private Pigeon target;

    private float next_shot_time;
    private float engage_timer;
    private bool can_engage { get { return engage_timer >= engage_delay; } }


    void Start()
    {
        target = GameManager.scene.pigeon;
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
			if (hit.tag == "Pigeon")
            {
                Vector3 normal = (hit.transform.position - transform.position).normalized;

				hit.GetComponent<Pigeon> ().Damage (shot_damage, hit.ClosestPoint(hit.transform.position), normal);
				hit.GetComponent<Pigeon> ().Daze ();
			}
        }

        float strength = Random.Range(min_shake_strength, max_shake_strength);
        float duration = Random.Range(min_shake_duration, max_shake_duration);

        AudioManager.PlayOneShot(flakk_clips[Random.Range(0, flakk_clips.Count)]);
        CameraShake.Shake(strength, duration);
    }


    void OnDrawGizmos()
    {
        Gizmos.color = can_engage ? Color.red : Color.green;
        Gizmos.DrawWireCube(transform.position, aiming_extents * 2);
    }

}
