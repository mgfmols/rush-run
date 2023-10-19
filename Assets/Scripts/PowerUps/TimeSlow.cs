using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSlow : MonoBehaviour, IPowerUp
{
    [SerializeField] float slowdownFactor = 0.2f;
    [SerializeField] float slowdownDuration = 7f;
    [SerializeField] float durability = 2;
    [SerializeField] readonly float maxDurability = 2;
    private bool active = false;

    void Update()
    {
        if (active)
        {
            // Slowly reverting time scale when active
            Time.timeScale += (1f / slowdownDuration) * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
        }
    }

    public bool Trigger(GameObject player)
    {
        // General check
        if (player == null)
        {
            return false;
        }
        // Setting global time scale slower for slow-motion effect
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * slowdownFactor/3;
        active = true;
        durability -= 1;
        StartCoroutine(End());
        return true;
    }

    public IEnumerator End()
    {
        // Waiting for *duration*
        yield return new WaitForSeconds(slowdownDuration * slowdownFactor);
        // Reset time scale
        active = false;
        Time.timeScale = 1;
        Time.fixedDeltaTime = Time.unscaledDeltaTime;
        Debug.Log("Time Slow stopped");
    }

    public float GetDurability()
    {
        return durability;
    }

    public float GetMaxDurability()
    {
        return maxDurability;
    }
}
