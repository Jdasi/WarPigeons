using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class TransformFollower : MonoBehaviour
{
    public Vector3 offset;
    public bool rotate;

    public Transform target
    {
        get
        {
            return target_;
        }

        set
        {
            target_ = value;
            this.gameObject.SetActive(true);
        }
    }

    [SerializeField] Transform target_;

    
    public void Update()
    {
        if (target == null)
        {
            this.gameObject.SetActive(false);
            return;
        }

        transform.position = new Vector3(target.transform.position.x,
            target.transform.position.y, target.transform.position.z) + offset;

        if (rotate)
        {
            transform.eulerAngles = new Vector3(25, target.eulerAngles.y, 0);
        }
    }
}
