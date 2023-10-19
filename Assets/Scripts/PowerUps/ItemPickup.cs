using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Inventory inventory;

    // Executes when item collides with object
    public void OnTriggerEnter(Collider other)
    {
        // If item collides with player
        if (other.Equals(player.GetComponent<Collider>()))
        {
            Pickup();
        }
    }

    private void Pickup()
    {
        if (inventory.PickupItem(gameObject))
        {
            // Making item invisible and untouchable when collided.
            // CANNOT DISABLE! Otherwise the powerup wont work.
            // The queue system used in powerups require objects to be active.
            gameObject.GetComponent<Renderer>().enabled = false;
            gameObject.GetComponent<MeshCollider>().enabled = false;
            Debug.Log("Player has picked up an item");
        }
        else
        {
            Debug.Log("Player already has an item");
            inventory.heldItemSlotAnimator.Play("Shake");
        }
    }
}
