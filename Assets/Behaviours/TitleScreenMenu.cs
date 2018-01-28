using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenMenu : MonoBehaviour
{
    [SerializeField] private int next_scene_build_index = 1;
    private bool loading = false;

	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButton("Controller 1 - A") && loading == false)
        {
            SceneManager.LoadSceneAsync(next_scene_build_index);
            loading = true;
        }
	}
}
