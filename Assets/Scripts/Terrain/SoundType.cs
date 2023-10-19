using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundType : MonoBehaviour
{
    [SerializeField] private float movementSpeedMultiplier;
    [SerializeField] private GroundType type;

    public float MovementSpeedMultiplier { get { return movementSpeedMultiplier; } set { movementSpeedMultiplier = value; } }
    public GroundType Type { get { return type; } set { type = value; } }
}
