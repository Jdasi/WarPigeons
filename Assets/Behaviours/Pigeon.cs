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

    [Header("References")]
    [SerializeField] Rigidbody rigid_body;

    private FlightMode target_mode;
    private bool transitioning { get { return current_mode != target_mode; } }

    private float dive_timer;
    


    void Start()
    {
        target_mode = current_mode;

        transform.position = new Vector3(transform.position.x, high_altitude, transform.position.z);
    }


    void Update()
    {
        if (!transitioning && Input.GetButtonDown("Controller 1 - Y"))
        {
            switch (current_mode)
            {
                case FlightMode.HIGH:
                {
                    dive_timer = 0;
                    target_mode = FlightMode.LOW;
                } break;

                case FlightMode.LOW:
                {
                    dive_timer = dive_duration;
                    target_mode = FlightMode.HIGH;
                } break;

                default: {} break;
            }
        }

        if (transitioning)
        {
            dive_timer += current_mode == FlightMode.HIGH ? Time.deltaTime : -Time.deltaTime;
        }
    }


    void FixedUpdate()
    {
        if (transitioning)
        {
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
