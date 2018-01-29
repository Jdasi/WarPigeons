using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static TempSceneRefs scene = new TempSceneRefs();

    private static GameManager instance;


    void Awake()
    {
        if (instance == null)
        {
            InitSingleton();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    void InitSingleton()
    {
        instance = this;

        DontDestroyOnLoad(this.gameObject);
        Cursor.visible = false;
        Application.targetFrameRate = 120;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            var fx = GameObject.FindObjectOfType<UnityEngine.PostProcessing.PostProcessingBehaviour>();
            fx.profile.motionBlur.enabled = !fx.profile.motionBlur.enabled;
        }
    }


    void LoadScene(int _index)
    {
        AudioManager.StopAllSFX();
        SceneManager.LoadScene(_index);
    }

}
