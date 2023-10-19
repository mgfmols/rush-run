using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum EventInfoName
{
    ButtonClick,
    ButtonClickBack,
    CountdownBeep,
    CountdownEnd,
    ButtonPop,
    FinalTime
}

[System.Serializable]
public class EventInfo
{
    public EventInfoName name;
    public EventReference soundEvent;
    public FMOD.Studio.EventInstance instance;
}

public class UISounds : MonoBehaviour
{
    [Header("Necessary")]
    [SerializeField] List<EventInfo> eventInfo;
    [SerializeField] GameObject _camera;

    void Start()
    {
        foreach(EventInfo info in eventInfo)
        {
            info.instance = RuntimeManager.CreateInstance(info.soundEvent);
            info.instance.set3DAttributes(RuntimeUtils.To3DAttributes(_camera.transform));
        }
    }

    void Update()
    {
        foreach (EventInfo info in eventInfo)
        {
            info.instance.set3DAttributes(RuntimeUtils.To3DAttributes(_camera.transform));
        }
    }

    public void PlaySound(int name)
    {
        foreach (EventInfo info in eventInfo)
        {
            if (((int)info.name).Equals(name))
            {
                info.instance.start();
            }
        }
    }
}
