using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using UnityEngine;

public class SpeedBoost : MonoBehaviour, IPowerUp
{
    [SerializeField] GameObject player;
    [SerializeField] float movementSpeedMultiplier = 2f;
    [SerializeField] float durability = 1f;
    [SerializeField] readonly float maxDurability = 1f;
    private float defaultMovementSpeed;

    public void Awake()
    {
        defaultMovementSpeed = player.GetComponent<Controls>().MovementSpeed;
    }

    public bool Trigger(GameObject player)
    {
        // General check
        if (player == null)
        {
            return false;
        }
        // Setting walking speed
        Controls controls = player.GetComponent<Controls>();
        if (controls.IsSprinting)
        {
            controls.MovementSpeed = defaultMovementSpeed * movementSpeedMultiplier * 2;
        }
        else
        {
            controls.MovementSpeed = defaultMovementSpeed * movementSpeedMultiplier;
        }

        durability -= 1;

        // Queuing speed buff reset
        StartCoroutine(End(player));
        Debug.Log("Speed Boost used");
        return true;
    }

    public IEnumerator End(GameObject player)
    {
        // General check
        if (player == null)
        {
            yield return null;
        }
        // Waiting 5 seconds
        yield return new WaitForSeconds(5);
        // Reset speed
        Controls controls = player.GetComponent<Controls>();
        if (controls.IsSprinting)
        {
            controls.MovementSpeed = defaultMovementSpeed * 2;
        }
        else
        {
            controls.MovementSpeed = defaultMovementSpeed;
        }

        Debug.Log("Speed Boost stopped");
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
