using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pigeon : MonoBehaviour
{
    public enum FlightMode
    {
        LOW,
        HIGH
    }

    public FlightMode flight_mode { get { return current_mode; } }

    [Header("Parameters")]
    [SerializeField] float high_altitude = 30;
    [SerializeField] float turn_speed = 80;
    [SerializeField] float move_speed = 40;

    [Space]
    [SerializeField] FlightMode current_mode = FlightMode.HIGH;
    [SerializeField] AnimationCurve dive_curve;
    [SerializeField] float dive_duration;

    [Space]
    [SerializeField] float high_flight_fov = 70;
    [SerializeField] float low_flight_fov = 60;

    [Header("References")]
    [SerializeField] Rigidbody rigid_body;
    [SerializeField] PigeonCamera pigeon_camera;

    private FlightMode target_mode;
    private bool transitioning { get { return current_mode != target_mode; } }

    private float dive_timer;
    


    void Start()
    {
        SetFlightMode(FlightMode.HIGH);

        transform.position = new Vector3(transform.position.x, high_altitude, transform.position.z);
    }


    void Update()
    {
        if (!transitioning && Input.GetButtonDown("Controller 1 - Y"))
        {
            ToggleFlightMode();
        }
    }


    void ToggleFlightMode()
    {
        switch (current_mode)
        {
            case FlightMode.HIGH:
            {
                SetFlightMode(FlightMode.LOW);
            } break;

            case FlightMode.LOW:
            {
                SetFlightMode(FlightMode.HIGH);
            } break;

            default: {} break;
        }
    }


    void SetFlightMode(FlightMode _mode)
    {
        switch (_mode)
        {
            case FlightMode.HIGH:
            {
                dive_timer = dive_duration;

                pigeon_camera.SetFOV(high_flight_fov);

                target_mode = FlightMode.HIGH;
            } break;

            case FlightMode.LOW:
            {
                dive_timer = 0;

                pigeon_camera.SetFOV(low_flight_fov);

                target_mode = FlightMode.LOW;
            } break;

            default: {} break;
        }
    }


    void FixedUpdate()
    {
        if (transitioning)
        {
            dive_timer += current_mode == FlightMode.HIGH ? Time.deltaTime : -Time.deltaTime;

            float curve_step = high_altitude * dive_curve.Evaluate(dive_timer / dive_duration);
            rigid_body.MovePosition(new Vector3(transform.position.x, curve_step, transform.position.z) + (transform.forward * move_speed * Time.deltaTime));

            if ((current_mode == FlightMode.HIGH && dive_timer >= dive_duration) ||
                (current_mode == FlightMode.LOW && dive_timer <= 0))
            {
                current_mode = target_mode;
            }
        }
        else
        {
            rigid_body.MovePosition(transform.position + (transform.forward * move_speed * Time.deltaTime));
        }

        if (!transitioning)
        {
            transform.Rotate(Vector3.up, Input.GetAxis("Controller 1 - Horizontal") * turn_speed * Time.deltaTime);
        }
    }

}
