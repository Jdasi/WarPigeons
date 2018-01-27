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
        GameManager.scene.message_spawner.MessageCollected();

        Destroy(this.gameObject);
    }

}
