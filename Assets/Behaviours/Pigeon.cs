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
    [SerializeField] float bank_angle = 25;
    [SerializeField] private float bank_speed = 100;

    [Space]
    [SerializeField] FlightMode current_mode = FlightMode.HIGH;
    [SerializeField] AnimationCurve dive_curve;
    [SerializeField] float dive_duration;

    [Space]
    [SerializeField] float high_flight_fov = 70;
    [SerializeField] float low_flight_fov = 60;

    [Space]
    [SerializeField] float high_flight_camera_speed = 1;
    [SerializeField] float low_flight_camera_speed = 5;

    [Header("References")]
    [SerializeField] Rigidbody rigid_body;
    [SerializeField] GameObject body;

    [HideInInspector] public Transform cam_follow_target;
    [HideInInspector] public Transform cam_lookat_target;

    private FlightMode target_mode;
    private bool transitioning { get { return current_mode != target_mode; } }

    private float dive_timer;
    private float horizontal;

    private Vector3 last_pos;
    

    void Start()
    {
        last_pos = transform.position;

        cam_follow_target = transform.Find("Camera Follow Target");
        cam_lookat_target = transform.Find("Camera Look Target");

        SetFlightMode(FlightMode.HIGH);

        transform.position = new Vector3(transform.position.x, high_altitude, transform.position.z);
    }


    void Update()
    {
        horizontal = Input.GetAxis("Controller 1 - Horizontal");

        if (transitioning)
        {
            Vector3 euler = new Vector3((last_pos - transform.position).y * 50, 0, 0);
            body.transform.localRotation = Quaternion.RotateTowards(body.transform.localRotation, Quaternion.Euler(euler), 300 * Time.deltaTime);
        }
        else
        {
            Vector3 euler = new Vector3(0, 0, -horizontal * bank_angle);
            body.transform.localRotation = Quaternion.RotateTowards(body.transform.localRotation, Quaternion.Euler(euler), bank_speed * Time.deltaTime);
        }

        if (!transitioning && Input.GetButtonDown("Controller 1 - Y"))
        {
            ToggleFlightMode();
        }

        last_pos = transform.position;
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

                GameManager.scene.pigeon_cam.SetFollowSpeed(high_flight_camera_speed);
                GameManager.scene.pigeon_cam.SetFOV(high_flight_fov);

                target_mode = FlightMode.HIGH;
            } break;

            case FlightMode.LOW:
            {
                dive_timer = 0;

                GameManager.scene.pigeon_cam.SetFollowSpeed(low_flight_camera_speed);
                GameManager.scene.pigeon_cam.SetFOV(low_flight_fov);

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
            transform.Rotate(Vector3.up, horizontal * turn_speed * Time.deltaTime);
        }
    }

}
