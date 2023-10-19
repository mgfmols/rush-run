using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterHandler : MonoBehaviour
{
    [SerializeField] GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.Equals(player.GetComponent<Collider>()))
        {
            Controls controls = player.GetComponent<Controls>();
            controls.MovementSpeed *= 0.5f;
            Physics.gravity *= 0.7f;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.Equals(player.GetComponent<Collider>()))
        {
            Controls controls = player.GetComponent<Controls>();
            controls.MovementSpeed /= 0.5f;
            Physics.gravity /= 0.7f;
        }
    }
}
