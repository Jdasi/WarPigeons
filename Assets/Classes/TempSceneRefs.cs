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

    public JobSystem job_system
    {
        get
        {
            if (job_system_ == null)
                job_system_ = GameObject.FindObjectOfType<JobSystem>();

            return job_system_;
        }
    }

    public UIManager ui_manager
    {
        get
        {
            if (ui_manager_ == null)
                ui_manager_ = GameObject.FindObjectOfType<UIManager>();

            return ui_manager_;
        }
    }

    private Pigeon pigeon_;
    private PigeonCamera pigeon_cam_;
    private JobSystem job_system_;
    private UIManager ui_manager_;
}
