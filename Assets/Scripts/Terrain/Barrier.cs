using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    [SerializeField] Material barrierMaterial;
    [SerializeField] float movementSpeedMultiplier = 0.5f;
    [SerializeField] GameObject player;
    private float defaultMovementSpeed;


    void Awake()
    {
        defaultMovementSpeed = player.GetComponent<Controls>().MovementSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (barrierMaterial.GetTextureOffset("_BaseMap").x >= 360)
        {
            barrierMaterial.SetTextureOffset("_BaseMap", Vector2.zero);
        }
        barrierMaterial.SetTextureOffset("_BaseMap", new Vector2(barrierMaterial.GetTextureOffset("_BaseMap").x + Time.deltaTime / 50, 0));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.Equals(player.GetComponent<Collider>()))
        {
            Controls controls = player.GetComponent<Controls>();
            if (controls.IsSprinting)
            {
                controls.MovementSpeed = defaultMovementSpeed * movementSpeedMultiplier * 2;
            }
            else
            {
                controls.MovementSpeed = defaultMovementSpeed * movementSpeedMultiplier;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.Equals(player.GetComponent<Collider>()))
        {
            StartCoroutine(End());
        }
    }

    public IEnumerator End()
    {
        yield return new WaitForSeconds(0.5f);
        Controls controls = player.GetComponent<Controls>();
        if (controls.IsSprinting)
        {
            controls.MovementSpeed = defaultMovementSpeed * 2;
        }
        else
        {
            controls.MovementSpeed = defaultMovementSpeed;
        }
    }
}
