using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobSystem : MonoBehaviour
{
    enum CurrentJob
    {
        COLLECTING,
        DELIVERING
    }

    [SerializeField] Transform message_spawns_container;
    [SerializeField] Transform delivery_points_container;

    private List<Transform> message_spawns = new List<Transform>();
    [SerializeField] GameObject message_prefab;

    private List<Transform> delivery_points = new List<Transform>();
    [SerializeField] GameObject delivery_point_prefab;

    private PigeonDestination current_destination;
    private CurrentJob current_job = CurrentJob.COLLECTING;

    int delivered_jobs;

    public void MessageCollected()
    {
        // TODO: record how many letters have been collected ?
        current_job = CurrentJob.DELIVERING;

        var pos = delivery_points[Random.Range(0, delivery_points.Count)].position;
        var clone = Instantiate(delivery_point_prefab, pos, Quaternion.identity);

        current_destination = clone.GetComponent<DeliveryPoint>();
        GameManager.scene.pigeon.SetDestination(current_destination);
    }


    public void MessageDelivered()
    {
        // TODO: record how many letters have been delivered ?
        delivered_jobs++;
        GameManager.scene.ui_manager.updateUIText(delivered_jobs);
        Debug.Log("Delivered");

        current_job = CurrentJob.COLLECTING;
        
        SpawnMessage();
    }


    void Start()
    {
        foreach (Transform child in message_spawns_container)
            message_spawns.Add(child);

        foreach (Transform child in delivery_points_container)
            delivery_points.Add(child);

        SpawnMessage();
    }


    void SpawnMessage()
    {
        var pos = message_spawns[Random.Range(0, message_spawns.Count)].position;
        var clone = Instantiate(message_prefab, pos, Quaternion.identity);

        current_destination = clone.GetComponent<MessageCollectible>();
        GameManager.scene.pigeon.SetDestination(current_destination);
    }


}
