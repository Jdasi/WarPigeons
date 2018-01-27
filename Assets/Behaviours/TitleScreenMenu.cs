using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenMenu : MonoBehaviour
{
    [SerializeField] private int next_scene_build_index = 1;

	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButton("Controller 1 - A"))
        {
            SceneManager.LoadSceneAsync(next_scene_build_index);
        }
	}
}
