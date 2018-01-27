using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class DamageFlash : MonoBehaviour
{
    public float flash_speed = 1;
    public float flash_ammount = 0.22f;
	public float redness = 0.5f;
	bool dead = false;

    [SerializeField] PostProcessingProfile camera_effects;
	
	
	void Update ()
	{
	    if (camera_effects == null)
	        return;

	    var vinette_settings = camera_effects.vignette.settings;
        vinette_settings.intensity = Mathf.Abs(Mathf.Sin(Time.realtimeSinceStartup) * flash_ammount);
	    camera_effects.vignette.settings = vinette_settings;

		var grading_settings = camera_effects.colorGrading.settings;
		if (dead) {
			grading_settings.channelMixer.blue = new Vector3 (0.5f, 0.5f, 0.5f);
			grading_settings.channelMixer.green = new Vector3 (0.5f, 0.5f, 0.5f);
			grading_settings.channelMixer.red = new Vector3 (0.5f, 0.5f, 0.5f);
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
		flash_speed = (400.0f - health * 2) / 100.0f;
		flash_ammount = (100.0f - health) / 225.0f;
		redness = (100.0f - health) / 400.0f;
	}

	public void Death()
	{
		dead = true;
	}

}
