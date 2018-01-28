using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour {

    [SerializeField] AudioSource source;
    [SerializeField] List<AudioClip> shot_clips;
    [SerializeField] Transform bulletPrefab;
    [SerializeField] float shoot_spread;
    [SerializeField] float stopFollowingRange;
    [SerializeField] float soldierRangeRadius;
    Transform target;
    bool lockedOn = false;

    float curShootTimer = 1;

    bool crouched = false;
    public float crouchIntervalMax = 5.0f;
    public float peekIntervalMax = 2.0f;
    float crouchTimer = 4.0f;
    Transform normalLegs, crouchLegs, m_view, m_gun;
    Light flash;
    float flashLength = 0.0f;
    bool flashInit = false;

    public int team = 0;
    List<Soldier> enemySoldiers;


    private void Start()
    {
        enemySoldiers = new List<Soldier>();
        foreach(Collider hit in Physics.OverlapSphere(transform.position, soldierRangeRadius))
        {
            Soldier enemy = hit.GetComponentInChildren<Soldier>();
            if(enemy != null)
            {
                if(enemy.team != team)
                {
                    enemySoldiers.Add(enemy);
                }
            }
        }

        flash = GetComponentInChildren<Light>();
        foreach (Transform child in gameObject.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.name == "NormalLegs")
            {
                normalLegs = child;
            }
            if (child.gameObject.name == "CrouchLegs")
            {
                crouchLegs = child;
            }
            if (child.gameObject.tag == "ViewCone")
            {
                m_view = child;
            }
            if (child.gameObject.tag == "Gun")
            {
                m_gun = child;
            }
        }

        crouchLegs.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(target != null)
        {
            Vector3 newLookAt = target.position;
            newLookAt.y = m_view.position.y;
            m_view.LookAt(newLookAt);
            m_gun.LookAt(target);
            Vector3 newGunPos = transform.position;
            newGunPos.y += 0.5f;
            if(crouched)
            {
                newGunPos.y -= 0.75f;
            }
            m_gun.position = newGunPos;
        }
        if (flashLength > 0.0f)
        {
            flash.enabled = true;
            if (flashInit)
            {
                flash.intensity -= 400.0f * Time.deltaTime;
                flash.range -= 100.0f * Time.deltaTime;
                flashLength -= 1000.0f * Time.deltaTime;
            }
            else
            {
                flashInit = true;
            }
            if (flashLength <= 0.0f)
            {
                flash.enabled = false;
            }
        }

        if (!lockedOn)
        {
            if(target == null && enemySoldiers.Count > 0)
            {
                target = enemySoldiers[Random.Range(0, enemySoldiers.Count - 1)].transform;
            }
            crouchTimer -= Time.deltaTime;
            if(crouchTimer <= 0.0f)
            {
                if (crouched)
                {
                    Uncrouch();
                    crouchTimer = Random.Range(0.75f, peekIntervalMax);
                }
                else
                {
                    Crouch();
                    crouchTimer = Random.Range(1.0f, crouchIntervalMax);
                }
            }
            //return;
        }
        else
        {
            Uncrouch();
        }

        Shoot();
    }

    Vector3 calculateLead()
    {
        Vector3 targetPos = target.position;
        float dist = Vector3.Distance(transform.position, target.position);

        targetPos += (target.forward * 0.2f) * dist;

        Vector3 variance = new Vector3(
            Random.Range(-shoot_spread, shoot_spread),
            Random.Range(-shoot_spread, shoot_spread),
            Random.Range(-shoot_spread, shoot_spread));
        targetPos += variance;

        return targetPos;
    }

    private void OnTriggerStay(Collider other)
    {
        if (lockedOn)
            return;

        if (other.tag == "Pigeon")
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, other.transform.position - transform.position, out hit))
            {
                if (hit.transform.tag == "Pigeon")
                {
                    lockedOn = true;
                    target = other.transform;
                    curShootTimer = 1.0f;
                }
            }
        }
    }

    void Shoot()
    {
        if (crouched || target == null) return;
        if (Vector3.Distance(transform.position, target.position) > stopFollowingRange)
        {
            lockedOn = false;
            target = null;
            curShootTimer = Random.Range(0.8f, 1.5f);
            return;
        }

        curShootTimer -= Time.deltaTime;

        if (curShootTimer > 0)
            return;

        curShootTimer = Random.Range(0.3f, 0.7f);

        Transform newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Vector3 lookAt = calculateLead();
        newBullet.GetComponent<TestBullet>().fireBullet(lookAt);

        //m_view.LookAt(lookAt);

        flashLength = Random.Range(100.0f, 250.0f);
        flash.intensity = Random.Range(6.0f, 12.0f);
        flash.range = Random.Range(5.0f, 10.0f);
        flashInit = false;

        var clip = shot_clips[Random.Range(0, shot_clips.Count)];

        source.PlayOneShot(clip);
    }

    void Crouch()
    {
        if (!crouched)
        {
            crouched = true;

            foreach (Transform child in transform)
            {
                child.Translate(new Vector3(0.0f, -0.75f, 0.0f));
            }

            normalLegs.gameObject.SetActive(false);
            crouchLegs.gameObject.SetActive(true);
        }
    }

    void Uncrouch()
    {
        if (crouched)
        {
            crouched = false;

            foreach (Transform child in transform)
            {
                child.Translate(new Vector3(0.0f, 0.75f, 0.0f));
            }

            normalLegs.gameObject.SetActive(true);
            crouchLegs.gameObject.SetActive(false);
        }
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = team == 0 ? Color.red : Color.blue;
        Gizmos.DrawSphere(transform.position, 3);
    }

}
