using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudManager : MonoBehaviour
{
    [SerializeField] VolumeRenderer cloud_1;
    [SerializeField] VolumeRenderer cloud_2;

    private static CloudManager instance;


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

        cloud_1.enabled = true;
        cloud_2.enabled = true;

        DontDestroyOnLoad(this.gameObject);
    }

}
