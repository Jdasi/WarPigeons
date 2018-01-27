using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Icon : MonoBehaviour 
{
    [SerializeField]
    private Renderer iconRenderer;
    [SerializeField]
    private bool active = false;



    private void OnEnable()
    {
        iconRenderer = GetComponent<Renderer>();
        iconRenderer.material.color = Color.green;

        active = true;
    }

    private void OnDisable()
    {
        active = false;
    }





    private void Update()
    {
        if(active)
        {
            transform.Rotate(Vector3.up * 3.5f * Time.deltaTime);
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Pigeon")
        {
            Debug.Log("Message Delivered");
        }
    }




}
