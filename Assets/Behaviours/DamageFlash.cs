using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class DamageFlash : MonoBehaviour
{
    public float flash_speed = 1;
    public float flash_ammount = 0.22f;

    [SerializeField] PostProcessingProfile camera_effects;
	
	
	void Update ()
	{
	    if (camera_effects == null)
	        return;

	    var vinette_settings = camera_effects.vignette.settings;
        vinette_settings.intensity = Mathf.Abs(Mathf.Sin(Time.realtimeSinceStartup) * flash_ammount);
	    camera_effects.vignette.settings = vinette_settings;
	}


    void OnDestroy()
    {
        var vinette_settings = camera_effects.vignette.settings;
        vinette_settings.intensity = 0;
        camera_effects.vignette.settings = vinette_settings;
    }
}
