using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using UnityEngine;

public interface IPowerUp
{
    bool Trigger(GameObject player);
    float GetDurability();
    float GetMaxDurability();
}
