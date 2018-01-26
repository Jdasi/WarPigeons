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
                pigeonPhysics.RollData(value);
                break;
            case GameAction.LS_Y_Axis:
                pigeonPhysics.PitchData(value);
                break;
            case GameAction.RS_X_Axis:
                break;
            case GameAction.RS_Y_Axis:
                break;
            case GameAction.LT_Axis:
                break;
            case GameAction.RT_Axis:
                pigeonPhysics.ThrustData(-value);
                break;
            case GameAction.LB_Down:
                break;
            case GameAction.LB_Held:
                pigeonPhysics.YawData(value);
                break;
            case GameAction.RB_Down:
                break;
            case GameAction.RB_Held:
                pigeonPhysics.YawData(value);
                break;
            case GameAction.A_Down:
                break;
            case GameAction.A_Held:
                break;
            case GameAction.B_Down:
                break;
            case GameAction.B_Held:
                break;
            case GameAction.None:
                break;
            default:
                break;
        }
    }
}
