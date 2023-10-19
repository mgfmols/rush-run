using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJump : MonoBehaviour, IPowerUp
{
    [SerializeField] float durability = 1;
    [SerializeField] readonly float maxDurability = 1;

    public bool Trigger(GameObject player)
    {
        // General check
        if (player == null)
        {
            return false;
        }
        // Enabling double jump
        Controls controls = player.GetComponent<Controls>();
        controls.CanDoubleJump = true;
        controls.UsedDoubleJump = false;
        durability -= 1;
        Debug.Log("Double Jump used");
        return true;
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
