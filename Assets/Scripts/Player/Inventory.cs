using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [Header("Inventory")]
    [SerializeField] GameObject heldItem;
    [Header("Canvas")]
    [SerializeField] public Animator heldItemSlotAnimator;
    [SerializeField] Animator itemSlotSectionAnimator;
    [SerializeField] GameObject heldItemSlot;
    [SerializeField] TMP_Text heldItemDurability;
    [SerializeField] GameObject itemFooter;
    [Header("Item Use")]
    [SerializeField] Texture keyTexture;
    [SerializeField] Texture keyPressedTexture;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // E key animation
            itemFooter.GetComponent<RawImage>().texture = keyPressedTexture;
            // Use item if you have one
            if (heldItem != null)
            {
                UseItem();
            }
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            // E key animation
            itemFooter.GetComponent<RawImage>().texture = keyTexture;
        }
    }

    public bool PickupItem(GameObject pickedUpItem)
    {
        if (heldItem != null)
        {
            return false;
        }
        // Setting item to stored data so it can be triggered
        heldItem = pickedUpItem;
        // Setting the item box display
        IPowerUp powerUp = heldItem.GetComponent<IPowerUp>();
        heldItemDurability.text = "×" + powerUp.GetDurability();
        heldItemSlot.GetComponent<MeshFilter>().mesh = pickedUpItem.GetComponent<MeshFilter>().mesh;
        // Animation
        if (itemSlotSectionAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            itemSlotSectionAnimator.Play("HeldItemSlot");
        }
        heldItemSlotAnimator.Play("Pop");
        return true;
    }

    public void UseItem()
    {
        IPowerUp powerUp = heldItem.GetComponent<IPowerUp>();
        if (powerUp.Trigger(gameObject))
        {
            // For Fuel \/
            // heldItemDurability.fillAmount = powerUp.getDurability() / powerUp.getMaxDurability();
            if (powerUp.GetDurability() == 0)
            {
                // Remove item from inventory
                heldItem = null;
                heldItemSlotAnimator.Play("Deplete");
                heldItemDurability.text = "";
            }
            else
            {
                // Remove 1 durability from item
                heldItemDurability.text = "×" + powerUp.GetDurability();
                heldItemSlotAnimator.Play("Use");
            }
        }
    }
}
