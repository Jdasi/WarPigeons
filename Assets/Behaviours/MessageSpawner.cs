using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageSpawner : MonoBehaviour
{
    [SerializeField] List<Transform> message_spawns;
    [SerializeField] GameObject message_prefab;

    private MessageCollectible current_collectible;


    void Start()
    {
        SpawnMessage();
    }


    void SpawnMessage()
    {
        var pos = message_spawns[Random.Range(0, message_spawns.Count)].position;
        var clone = Instantiate(message_prefab, pos, Quaternion.identity);

        current_collectible = clone.GetComponent<MessageCollectible>();
        GameManager.scene.pigeon.MessageSpawned(current_collectible);
    }

}
