using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBullet : MonoBehaviour {

    bool fired = false;
   
    Rigidbody m_RB;

    private void Start()
    {
        m_RB = GetComponent<Rigidbody>();
        fired = true;
    }


    public void fireBullet(Vector3 _targetPos)
    {
        transform.LookAt(_targetPos);
    }

    private void FixedUpdate()
    {
        if (!fired)
            return;

        m_RB.MovePosition(transform.position + (transform.forward * 1.8f));
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Pigeon")
        {
            Destroy(gameObject);
        }
        else if (other.tag != "SquadMan")
        {
            //Destroy(gameObject);
        }
    }
}
