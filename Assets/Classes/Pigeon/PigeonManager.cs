using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigeonManager : MonoBehaviour
{
    [SerializeField]
    private PigeonPhysics pigeonPhysics;
    [SerializeField]
    private ControllerID controllerID;



    private void Awake()
    {
        pigeonPhysics = GetComponent<PigeonPhysics>();
        if(!pigeonPhysics)
        {
            Debug.Log("ERROR: PigeonPhysics script not added to the gameobject");
        }
    }



    private void OnEnable()
    {
        InputManager.inputDetected += HandleInput;
    }



    private void OnDisable()
    {
        InputManager.inputDetected -= HandleInput;
    }



    private void HandleInput(GameAction gameAction, float value, ControllerID gamepadID)
    {
        if(gamepadID != controllerID) return;

        switch(gameAction)
        {
            case GameAction.LS_X_Axis:
                pigeonPhysics.YawData(value);
                break;
            case GameAction.RT_Axis:
                pigeonPhysics.ThrustData(-value);
                break;
            case GameAction.LB_Down:
                break;
            case GameAction.LB_Held:
                break;
            case GameAction.RB_Held:
                
                break;
        }
    }
}
