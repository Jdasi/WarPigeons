using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempParticle : MonoBehaviour
{

    void Start()
    {
        ParticleSystem system = GetComponent<ParticleSystem>();

        if (system == null)
            system = GetComponentInChildren<ParticleSystem>();

        StartCoroutine(CleanUp(system));
    }


    IEnumerator CleanUp(ParticleSystem _system)
    {
        yield return new WaitForSeconds(_system.main.duration);
        _system.Stop();

        yield return new WaitUntil(() => !_system.IsAlive());
        Destroy(this.gameObject);
    }

}
