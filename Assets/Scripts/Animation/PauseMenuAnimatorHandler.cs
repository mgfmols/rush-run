using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PauseMenuAnimatorHandler : MonoBehaviour
{

    public void Appear()
    {
        gameObject.GetComponent<RectTransform>().localScale = Vector3.one;
    }

    public void Disappear()
    {
        gameObject.GetComponent<RectTransform>().localScale = Vector3.zero;
    }
}
