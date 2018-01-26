using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TargetPapers : MonoBehaviour 
{
    [SerializeField]
    private GameObject targetObject;
    [SerializeField]
    private GameObject iconPrefab;

    public bool TargetRecieved { get; internal set; }
    public bool DisplayTarget { get; set; }




    private void Awake()
    {
        DisplayTarget = true;
    }

    private void Update()
    {
        if(DisplayTarget)
        {
            if(!gameObject.activeInHierarchy)
            {
                gameObject.SetActive(true);
            }
        }
        else
        {
            if(gameObject.activeInHierarchy)
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Pigeon")
        {
            Debug.Log("MESSAGE RECIEVED");

            TargetRecieved = true;
            DisplayTarget = false;

            LocateTarget();
        }
    }


    private void LocateTarget()
    {
        var targetIcon = Instantiate(iconPrefab, 
            new Vector3(targetObject.transform.position.x, targetObject.transform.position.y + 50f,  targetObject.transform.position.z),
            Quaternion.identity);
    }
}

