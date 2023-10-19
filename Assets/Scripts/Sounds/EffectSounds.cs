using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSounds : MonoBehaviour
{
    [SerializeField] Camera _camera;
    [SerializeField] Rigidbody playerBody;
    [SerializeField] EventReference reference;
    [SerializeField] EventInstance instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = RuntimeManager.CreateInstance(reference);
        instance.set3DAttributes(RuntimeUtils.To3DAttributes(_camera.transform));
        instance.start();
    }

    // Update is called once per frame
    void Update()
    {
        instance.set3DAttributes(RuntimeUtils.To3DAttributes(_camera.transform));
        instance.setParameterByName("Velocity", playerBody.velocity.magnitude);
    }

    public void Pause()
    {
        instance.setPaused(true);
    }

    public void UnPause()
    {
        instance.setPaused(false);
    }
}
