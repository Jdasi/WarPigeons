using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class DamageFlash : MonoBehaviour
{
    public float flash_speed = 1;
    public float flash_ammount = 0.22f;
	public float redness = 0.5f;
	float dazed = 0.0f;
	bool dead = false;

    [SerializeField] PostProcessingProfile camera_effects;
	
	
	void Update ()
	{
	    if (camera_effects == null)
	        return;

	    var vinette_settings = camera_effects.vignette.settings;
		vinette_settings.intensity = flash_ammount * 0.25f + Mathf.Abs(Mathf.Sin(Time.realtimeSinceStartup * flash_speed)) * flash_ammount * 0.75f;
		if (dazed > 0.0f) {
			vinette_settings.color = new Color (0.1f, 0.0f, 0.5f);
		} else {
			vinette_settings.color = new Color (0.1f, 0.0f, 0.0f);
		}
	    camera_effects.vignette.settings = vinette_settings;

		var grading_settings = camera_effects.colorGrading.settings;
		if (dead) {
			Vector3 mid = new Vector3(0.5f, 0.5f, 0.5f);
			Vector3 newBlue = Vector3.Lerp (grading_settings.channelMixer.blue, mid, Time.deltaTime);
			Vector3 newGreen = Vector3.Lerp(grading_settings.channelMixer.green, mid, Time.deltaTime);
			Vector3 newRed = Vector3.Lerp(grading_settings.channelMixer.red, mid, Time.deltaTime);
			grading_settings.channelMixer.blue = newBlue;
			grading_settings.channelMixer.green = newGreen;
			grading_settings.channelMixer.red = newRed;
			if (flash_ammount >= 0.0f) {
				flash_ammount -= Time.deltaTime;
				if (flash_ammount < 0.0f)
					flash_ammount = 0.0f;
			}
		} else {
			grading_settings.channelMixer.blue = new Vector3 (0f, 0f, 1f - redness);
			grading_settings.channelMixer.green = new Vector3 (0f, 1f - redness, 0f);
			grading_settings.channelMixer.red = new Vector3 (1f, redness, redness);
		}
		camera_effects.colorGrading.settings = grading_settings;
	}


    void OnDestroy()
    {
        var vinette_settings = camera_effects.vignette.settings;
        vinette_settings.intensity = 0;
        camera_effects.vignette.settings = vinette_settings;

		var grading_settings = camera_effects.colorGrading.settings;
		grading_settings.channelMixer.blue = new Vector3 (0f, 0f, 1f);
		grading_settings.channelMixer.green = new Vector3 (0f, 1f, 0f);
		grading_settings.channelMixer.red = new Vector3 (1f, 0f, 0f);
		camera_effects.colorGrading.settings = grading_settings;
    }

	public void UpdateDamage(float health)
	{
		flash_speed = (150.0f - health) / 50.0f;
		//flash_ammount = (100.0f - health) / 200.0f;
		float aaa = ((100.0f - health) / 100.0f);
		if (aaa > 0.0f)
			aaa = Mathf.Sqrt(Mathf.Sqrt (aaa));
		flash_ammount = 0.45f * aaa;
		redness = (100.0f - health) / 400.0f;
	}

	public void UpdateDaze(float daze)
	{
		dazed = daze;
	}

	public void Death()
	{
		dead = true;
	}

}
