using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MobileControl : MonoBehaviour
{
    public enum ControlState
    {
        GAMEPLAY,
        GAMEOVER
    }

    [SerializeField] float left_threshold = 0.25f;
    [SerializeField] float right_threshold = 0.75f;

    private float left_point;
    private float right_point;

    public ControlState state = ControlState.GAMEPLAY;


    void Awake()
    {
        left_point = Screen.width * left_threshold;
        right_point = Screen.width * right_threshold;
    }


    void Update()
    {
        switch (state)
        {
            case ControlState.GAMEPLAY:
            {
                GameplayUpdate();
            } break;

            case ControlState.GAMEOVER:
            {
                GameOverUpdate();
            } break;
        }
    }


    void GameplayUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            var mouse_pos = Input.mousePosition;

            if (mouse_pos.x < left_point)
            {
                GameManager.scene.pigeon.TurnLeft();
            }
            else if (mouse_pos.x > right_point)
            {
                GameManager.scene.pigeon.TurnRight();
            }
            else
            {
                GameManager.scene.pigeon.ToggleFlightMode();
            }
        }
        else
        {
            GameManager.scene.pigeon.StopTurning();
        }
    }


    void GameOverUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            var mouse_pos = Input.mousePosition;

            if (mouse_pos.x < left_point)
            {
                SceneManager.LoadScene(0);
            }
            else if (mouse_pos.x > right_point)
            {
                SceneManager.LoadScene(1);
            }
        }
    }

}
