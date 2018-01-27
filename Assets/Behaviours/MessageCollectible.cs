using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageCollectible : MonoBehaviour
{

    void OnTriggerEnter(Collider _other)
    {
        if (!_other.CompareTag("Pigeon"))
            return;

        Debug.Log("Message collected");
        Destroy(this.gameObject);
    }

}
