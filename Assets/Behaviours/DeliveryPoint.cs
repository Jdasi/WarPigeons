﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryPoint : PigeonDestination
{

    void OnTriggerEnter(Collider _other)
    {
        if (!_other.CompareTag("Pigeon"))
            return;

        GameManager.scene.pigeon.SetLetterEquipped(false);
        GameManager.scene.message_spawner.MessageDelivered();

        Destroy(this.gameObject);
    }

}
