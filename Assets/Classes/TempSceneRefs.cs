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

    private Pigeon pigeon_;
    private PigeonCamera pigeon_cam_;

}
