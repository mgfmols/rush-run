using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum GroundType
{
    Grass,
    Farmland,
    Path,
    Wood
}

public class FootstepSoundScript : MonoBehaviour
{
    [Header("Necessary")]
    [SerializeField] EventReference soundEvent;
    [SerializeField] Controls controls;
    [SerializeField] float speed = 0.4f;

    FMOD.Studio.EventInstance instance;
        
    void Start()
    {
        instance = RuntimeManager.CreateInstance(soundEvent);
        instance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject.transform));
        InvokeRepeating("PlayFootsteps", 0, speed);
    }

    void Update()
    {
        instance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject.transform));
    }

    public void SetNewSpeed(float newSpeed)
    {
        CancelInvoke("PlayFootsteps");
        speed = newSpeed;
        InvokeRepeating("PlayFootsteps", 0, speed);
    }

    public void UpdateGroundType(GroundType type)
    {
        instance.setParameterByName("GroundType", (int) type);
    }

    void PlayFootsteps()
    {
        if ((controls.RigidBody.velocity.x >= 0.01 || controls.RigidBody.velocity.x <= -0.01 || controls.RigidBody.velocity.z >= 0.01 || controls.RigidBody.velocity.z <= -0.01) && !controls.CheckIsJumping())
        {
            instance.start();
        }
    }
}
