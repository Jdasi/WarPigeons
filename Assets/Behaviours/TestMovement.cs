using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovement : MonoBehaviour {

    enum PigeonHeight
    {
        TOP,
        BOTTOM
    }

    [SerializeField] float m_topHeight, m_bottomHeight;

    PigeonHeight m_curHeight;

    [SerializeField]bool movingUp = false, movingDown = false;

    Rigidbody m_RB;

    private void Start()
    {
        m_RB = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            switch (m_curHeight)
            {
                case PigeonHeight.TOP:
                    movingUp = false;
                    movingDown = true;
                    m_curHeight = PigeonHeight.BOTTOM;
                    break;
                case PigeonHeight.BOTTOM:
                    movingUp = true;
                    movingDown = false;
                    m_curHeight = PigeonHeight.TOP;
                    break;
                default:
                    break;
            }
        }
    }

    private void FixedUpdate()
    {
        if (movingUp)
        {
            if (transform.position.y < m_topHeight)
            {
                m_RB.AddForce(Vector3.up * 20, ForceMode.VelocityChange);
            }
            else
            {
                movingUp = false;
                m_RB.velocity = new Vector3(m_RB.velocity.x, 0, m_RB.velocity.z);
            }
        }
        if (movingDown)
        {
            if (transform.position.y > m_bottomHeight)
            {
                m_RB.AddForce(Vector3.down * 20, ForceMode.VelocityChange);
            }
            else
            {
                movingDown = false;
                m_RB.velocity = new Vector3(m_RB.velocity.x, 0, m_RB.velocity.z);
            }
        }

        m_RB.MovePosition(transform.position + (transform.forward * 0.5f));

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.up, -2f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up, 2f);
        }
    }
}
