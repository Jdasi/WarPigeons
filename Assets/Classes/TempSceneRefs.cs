using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TempSceneRefs
{
    public Pigeon pigeon
    {
        get
        {
            if (pigeon_ == null)
                pigeon_ = GameObject.FindObjectOfType<Pigeon>();
    
            return pigeon_;
        }
    }

    public PigeonCamera pigeon_cam
    {
        get
        {
            if (pigeon_cam_ == null)
                pigeon_cam_ = GameObject.FindObjectOfType<PigeonCamera>();

            return pigeon_cam_;
        }
    }

    public JobSystem message_spawner
    {
        get
        {
            if (message_spawner_ == null)
                message_spawner_ = GameObject.FindObjectOfType<JobSystem>();

            return message_spawner_;
        }
    }

    private Pigeon pigeon_;
    private PigeonCamera pigeon_cam_;
    private JobSystem message_spawner_;

}
