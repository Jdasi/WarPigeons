using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

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
    [SerializeField] float bank_speed = 100;

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

    [Space]
    [SerializeField] GameObject ragdoll_prefab;

    [Space]
    [SerializeField] float dist_before_vibrate = 200;

    [Header("References")]
    [SerializeField] Rigidbody rigid_body;
    [SerializeField] GameObject body;
    [SerializeField] GameObject letter_obj;
    [SerializeField] Transform letter_drop_obj;
    [SerializeField] private GameObject feather_prefab;

    [HideInInspector] public Transform cam_follow_target;
    [HideInInspector] public Transform cam_lookat_target;

    private FlightMode target_mode;
    private bool transitioning { get { return current_mode != target_mode; } }

    private float dive_timer;
    private float horizontal;

    private Vector3 last_pos;
    private PigeonDestination destination;
	private DamageFlash damage_camera;

	private float health = 100.0f;
	private float damaged = 0.0f;
	private float dazed = 0.0f;

	
	public void Damage(float _amount, Vector3 _hit_pos, Vector3 _hit_nrml)
	{
		if (health <= 0.0f)
			return;
		health -= _amount;
		damaged = 1.0f;
		if (health <= 0.0f)
			Kill ();
        SpawnFeathers(_hit_pos, _hit_nrml);
	}

    private void SpawnFeathers(Vector3 _hit_pos, Vector3 _hit_nrml)
    {
        Transform feather_clone = Instantiate(feather_prefab).transform;
        feather_clone.position = transform.position;
        feather_clone.LookAt(_hit_nrml.normalized);
    }

	public void Daze()
	{
		dazed = 2.0f;
	}
	
	public void SetDestination(PigeonDestination _destination)
    {
        destination = _destination;
    }


    public void SetLetterEquipped(bool _equipped)
    {
        letter_obj.SetActive(_equipped);

        if (!_equipped)
            DropLetter();
    }


    public void Kill()
    {
        this.enabled = false;
        body.SetActive(false);

        var clone = Instantiate(ragdoll_prefab, body.transform.position, body.transform.rotation);
        var rb = clone.GetComponent<Rigidbody>();

        rb.velocity = body.transform.forward * move_speed;
        rb.AddForce(body.transform.forward * 30, ForceMode.Impulse);
        rb.AddTorque(Random.Range(0, 25), Random.Range(0, 25), Random.Range(0, 25));

        GameManager.scene.pigeon_cam.SetTargets(rb.transform, rb.transform);
        GameManager.scene.pigeon_cam.SetFollowSpeed(0.5f);
        GameManager.scene.pigeon_cam.SetFOV(80);
        GameManager.scene.pigeon_cam.follow_offset = new Vector3(0, 5, -5);

        Time.timeScale = 0.75f;

		damage_camera.Death ();

        StartCoroutine("showEndScreenAfterDelay");
    }

    IEnumerator showEndScreenAfterDelay()
    {
        yield return new WaitForSeconds(2.5f);
        GameManager.scene.ui_manager.displayEndScreen();
        yield break;
    }

    void Start()
    {
		damage_camera = GetComponent<DamageFlash>();
		damage_camera.UpdateDamage (health);

        AudioManager.SetAmbience(AmbienceType.ABOVE);

        last_pos = transform.position;

        cam_follow_target = transform.Find("Camera Follow Target");
        cam_lookat_target = transform.Find("Camera Look Target");

        SetFlightMode(FlightMode.HIGH);

        transform.position = new Vector3(transform.position.x, high_altitude, transform.position.z);
    }


    void Update()
    {
		if (health > 0.0f && health < 100.0f) {
			if (damaged > 0.0f) {
				damaged -= Time.deltaTime;
			} else {
				health += Time.deltaTime * 5.0f;
				if (health > 100.0f)
					health = 100.0f;
			}

			damage_camera.UpdateDamage (health);
			damage_camera.UpdateDaze (dazed);
		}

        horizontal = Input.GetAxis("Controller 1 - Horizontal");
		if (dazed > 0.0f) {
			horizontal *= -1;
			dazed -= Time.deltaTime;
		}

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

        if (Input.GetKeyDown(KeyCode.K))
        {
			Damage (10.0f, transform.position, Vector3.up);
        }
		if (Input.GetKeyDown(KeyCode.L))
		{
			Daze ();
		}

        HandleMessageProximity();
    }

    void DropLetter()
    {
        Transform dropScroll = Instantiate(letter_drop_obj, letter_obj.transform.position, letter_obj.transform.rotation);
        dropScroll.GetComponent<Rigidbody>().AddForce(transform.forward * 20, ForceMode.Impulse);
        dropScroll.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-30,30), Random.Range(-30, 30), Random.Range(-30, 30)), ForceMode.Impulse);
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
                GameManager.scene.pigeon_cam.follow_offset = Vector3.zero;

                target_mode = FlightMode.HIGH;
                AudioManager.SetAmbience(AmbienceType.ABOVE);
            } break;

            case FlightMode.LOW:
            {
                dive_timer = 0;

                GameManager.scene.pigeon_cam.SetFollowSpeed(low_flight_camera_speed);
                GameManager.scene.pigeon_cam.SetFOV(low_flight_fov);
                GameManager.scene.pigeon_cam.follow_offset = new Vector3(0, -10, 0);

                target_mode = FlightMode.LOW;
                AudioManager.SetAmbience(AmbienceType.BELOW);
            } break;

            default: {} break;
        }
    }


    void HandleMessageProximity()
    {
        if (destination == null || flight_mode != FlightMode.HIGH || transitioning)
        {
            SetVibration(0, 0);
            return;
        }

        float dist = Vector3.Distance(transform.position, destination.transform.position);
        if (dist > dist_before_vibrate)
        {
            SetVibration(0, 0);
            return;
        }

        float vibration_amount = (dist_before_vibrate - dist) * 0.001f;
        vibration_amount = Mathf.Clamp(vibration_amount, 0, 0.2f);
        SetVibration(vibration_amount, vibration_amount);
    }


    void SetVibration(float _left, float _right)
    {
        var player = ReInput.players.GetPlayer(0);
        foreach(Joystick j in player.controllers.Joysticks)
        {
            if (!j.supportsVibration)
                continue;

            j.SetVibration(_left, _right);
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
            float trigger_axis = Input.GetAxis("Controller 1 - Trigger");
            float modifier = 1 + (trigger_axis * 0.75f);

            transform.Rotate(Vector3.up, horizontal * turn_speed * modifier * Time.deltaTime);
        }
    }

	void OnCollisionEnter(Collision collision)
	{
		Damage (100.0f, collision.contacts[0].point, collision.contacts[0].point);
	}


    void OnDisable()
    {
        SetVibration(0, 0);
    }


    void OnDestroy()
    {
        SetVibration(0, 0);
    }

}
