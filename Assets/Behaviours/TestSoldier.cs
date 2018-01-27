using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSoldier : MonoBehaviour {

    [SerializeField] Transform bulletPrefab;
    [SerializeField] float shoot_spread;
    [SerializeField] float stopFollowingRange;
    Transform target;
    bool lockedOn = false;

    float curShootTimer = 1;

    private void Update()
    {
        if (!lockedOn)
            return;

        if(Vector3.Distance(transform.position, target.position) > stopFollowingRange)
        {
            lockedOn = false;
            target = null;
            curShootTimer = 1;
            return;
        }

        curShootTimer -= Time.deltaTime;

        if (curShootTimer > 0)
            return;

        curShootTimer = 0.2f;

        Transform newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        newBullet.GetComponent<TestBullet>().fireBullet(calculateLead());
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
            if(Physics.Raycast(transform.position, other.transform.position - transform.position, out hit))
            {
                if(hit.transform.tag == "Pigeon")
                {
                    lockedOn = true;
                    target = other.transform;
                }
            }
        }
    }
}
