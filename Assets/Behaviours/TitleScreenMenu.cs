﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenMenu : MonoBehaviour
{
    [SerializeField] float enter_scene_fade = 1;
    [SerializeField] float exit_scene_fade = 2;

    [SerializeField] private int next_scene_build_index = 1;
    private bool loading = false;

    [SerializeField] FadableGraphic world_fade;
    [SerializeField] FadableGraphic canvas_fade;


    void Start()
    {
        world_fade.FadeOut(enter_scene_fade);
        canvas_fade.FadeOut(enter_scene_fade);
    }

	
	// Update is called once per frame
	void Update ()
    {
        if (!loading && Input.GetButton("Controller 1 - A"))
        {
            loading = true;
            APressed();
        }
	}


    void APressed()
    {
        AudioManager.PlayOneShot("pigeonTakeoff-002");

        world_fade.FadeIn(exit_scene_fade);
        canvas_fade.FadeIn(exit_scene_fade);

        Invoke("LoadScene", exit_scene_fade);
    }


    void LoadScene()
    {
        SceneManager.LoadSceneAsync(next_scene_build_index);
    }

}
