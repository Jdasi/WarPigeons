using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Put a black boarder around the split screen camera. 

public class PigeonCam : MonoBehaviour 
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private PigeonPhysics aircraftPhysics;
    [SerializeField]
    private float followDistance = 3f;
    [SerializeField]
    private float cameraElevation = 3f;
    [SerializeField]
    private float followTightness = 5f; 
    [SerializeField]
    private float rotationTightness = 10f;
    [SerializeField]
    private float afterburnShakeAmount = 2f;
    [SerializeField]
    private float yawMultiplier = 0.005f;


    private void Start()
    {
        aircraftPhysics = FindObjectOfType<PigeonPhysics>();
        if(!aircraftPhysics)
        {
            Debug.LogError("(Flight Controls) Flight controller is null on camera!");
            return;
        }

        target = GameObject.FindGameObjectWithTag("Aircraft Camera Target").transform;
        if(!target)
        {
            Debug.LogError("(Flight Controls) Camera target is null!");
            return;
        }
    }


    private void FixedUpdate()
    {
        //Calculate where we want the camera to be.
        Vector3 newPosition = target.TransformPoint(aircraftPhysics.Yaw * yawMultiplier, cameraElevation, -followDistance);

        //Get the difference between the current location and the target's current location.
        Vector3 positionDifference = target.position - transform.position;
        
        //Move the camera towards the new position.
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.fixedDeltaTime * followTightness);

        Quaternion newRotation;
        if(aircraftPhysics.AfterBurnerActive)
        {
            // TODO: Make shake amount adjust based on the Aircrafts velocity.

            newRotation = Quaternion.LookRotation(positionDifference + new Vector3(
                Random.Range(-afterburnShakeAmount, afterburnShakeAmount),
                Random.Range(-afterburnShakeAmount, afterburnShakeAmount),
                Random.Range(-afterburnShakeAmount, afterburnShakeAmount)),
                target.up);
        }
        else
        {
            newRotation = Quaternion.LookRotation(positionDifference, target.up);
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.fixedDeltaTime * rotationTightness);
    }
}
