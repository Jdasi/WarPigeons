using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [System.Diagnostics.Conditional("DEBUG")]

public enum GameAction
{
    LS_X_Axis,
    LS_Y_Axis,
    RS_X_Axis,
    RS_Y_Axis,
    LT_Axis,
    RT_Axis,
    LB_Down,
    LB_Held,
    RB_Down,
    RB_Held,
    A_Down,
    A_Held,
    B_Down,
    B_Held,
    None
}


public enum ControllerID
{
    One = 1,
    Two = 2,
    Three = 3,
    Four = 4,
    Unassigned = 0
}


public enum ControllerStatus
{
    Reconnected,
    Disconnected
}


public class InputManager : MonoBehaviour
{
    public delegate void InputDetected(GameAction gameAction, float value, ControllerID gamepadID);
    public static event  InputDetected inputDetected;

    public delegate void ControllerStatusChange(ControllerStatus controllerStatus, ControllerID gamepadID);
    public static event ControllerStatusChange controllerStatusChange;


    private ControllerID controllerID = ControllerID.Unassigned;
    private const int MAX_CONTROLLERS = 4;

    [SerializeField]
    private List<string> previousConnectedControllers = new List<string>(); 
    [SerializeField]
    private string[] controllerNames = new string[MAX_CONTROLLERS];




    private void Awake()
    {
        controllerNames = Input.GetJoystickNames();
        Debug.Log("MESSAGE: Initial number of joysticks " + controllerNames.Length);
        if(previousConnectedControllers.Count <= 0)
        {
            previousConnectedControllers = new List<string>(controllerNames);
        }
    }

    private void Update()
    {
        controllerNames = Input.GetJoystickNames();
        if(controllerNames.Length > MAX_CONTROLLERS)
        {
            Debug.Log("ERROR: The MAX number of controllers connected has been exceeded");
        }

        for(var i = 0; i < controllerNames.Length; i++)
        {
            controllerID = (ControllerID)(i + 1);
            
            UpdateControllersStatus(i);

            AllAxis();
            AllButtons();
        }
    }




    /// <summary>
    /// Use the Joystick Names function to check if a controller has connected or disconnected.
    /// </summary>
    private void UpdateControllersStatus(int listID)
    {
        var controller = controllerNames[listID];
        if(controller != previousConnectedControllers[listID])
        {
            if(controller == "")
            {
                Debug.Log("Controller " + controllerID.ToString() + " has disconnected");
                controllerStatusChange(ControllerStatus.Disconnected, controllerID);
            }
            else
            {
                Debug.Log("Controller " + controllerID.ToString() + " has connected");
                controllerStatusChange(ControllerStatus.Reconnected, controllerID);
            }
        }
        previousConnectedControllers[listID] = controller;
    }



    private void AllAxis()
    {
        LeftStick();
        RightStick();
        Triggers();
    }

    private void LeftStick()
    {
        var x = Input.GetAxis("Controller " + (int)controllerID + " - Horizontal");
        inputDetected(GameAction.LS_X_Axis, x, controllerID);

        var y = Input.GetAxis("Controller " + (int)controllerID + " - Vertical");
        inputDetected(GameAction.LS_Y_Axis, y, controllerID);
    }

    private void RightStick()
    {
        var x = Input.GetAxis("Controller " + (int)controllerID + " - Second Horizontal");
        inputDetected(GameAction.RS_X_Axis, x, controllerID);

        var y = Input.GetAxis("Controller " + (int)controllerID + " - Second Vertical");
        inputDetected(GameAction.RS_Y_Axis, y, controllerID);
    }
  
    private void Triggers()
    {
        var trigger = Input.GetAxis("Controller " + (int)controllerID + " - Trigger");
       
        if(trigger > 0)
        {
            inputDetected(GameAction.LT_Axis, trigger, controllerID);
        }
        else if (trigger < 0)
        {
            inputDetected(GameAction.RT_Axis, trigger, controllerID);
        }
        else
        {
            inputDetected(GameAction.RT_Axis, trigger, controllerID);
        }
    }




    private void AllButtons()
    {
        ADown();
        AHeld();

        BDown();
        BHeld();

        Bumpers();
    }

    private void ADown()
    {
        var action = Input.GetButtonDown("Controller " + (int)controllerID + " - A");
        if(action)
        {
            inputDetected(GameAction.A_Down, 1, controllerID);
        }
    }

    private void AHeld()
    {
        var action = Input.GetButton("Controller " + (int)controllerID + " - A");
        if(action)
        {
            inputDetected(GameAction.A_Held, 1, controllerID);
        }
    }

    private void BDown()
    {
        var action = Input.GetButtonDown("Controller " + (int)controllerID + " - B");
        if(action)
        {
            inputDetected(GameAction.B_Down, 1, controllerID);
        }
        else
        {
            inputDetected(GameAction.B_Down, 0, controllerID);
        }
    }

    private void BHeld()
    {
        var action = Input.GetButton("Controller " + (int)controllerID + " - B");
        if(action)
        {
            inputDetected(GameAction.B_Held, 1, controllerID);
        }
        else
        {
            inputDetected(GameAction.B_Held, 0, controllerID);
        }
    }

    private void LBDown()
    {
        var action = Input.GetButtonDown("Controller " + (int)controllerID + " - LeftBumper");
        if(action)
        {
            inputDetected(GameAction.LB_Down, -1, controllerID);
        }
    }

    private void RBDown()
    {
        var action = Input.GetButtonDown("Controller " + (int)controllerID + " - RightBumper");
        if(action)
        {
            inputDetected(GameAction.RB_Down, 1, controllerID);
        }
    }

    private void Bumpers()
    {        
        var leftBumper  = Input.GetButton("Controller " + (int)controllerID + " - LeftBumper");
        var rightBumper = Input.GetButton("Controller " + (int)controllerID + " - RightBumper");

        if(leftBumper)
        {
            inputDetected(GameAction.LB_Held, -1, controllerID);
        }

        if(rightBumper)
        {
            inputDetected(GameAction.RB_Held, 1, controllerID);
        }

        if(!leftBumper && !rightBumper)
        {
            inputDetected(GameAction.LB_Held, 0, controllerID);
            inputDetected(GameAction.RB_Held, 0, controllerID);
        }
    }



    public ControllerID GetControllerID(int ID)
    {
        Debug.Log("MESSAGE: Number of controllers connected: " + controllerNames.Length);
        if(ID > controllerNames.Length)
        {
            Debug.Log("MESSAGE: Run out of connected controllers. Returning unassigned");
            return ControllerID.Unassigned;
        }
        return (ControllerID)ID;
    }
}
