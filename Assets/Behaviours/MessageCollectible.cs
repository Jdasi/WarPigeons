using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageCollectible : PigeonDestination
{

    void OnTriggerEnter(Collider _other)
    {
        if (!_other.CompareTag("Pigeon"))
            return;

        GameManager.scene.pigeon.SetLetterEquipped(true);
        GameManager.scene.job_system.MessageCollected();

        Destroy(this.gameObject);
    }

}
