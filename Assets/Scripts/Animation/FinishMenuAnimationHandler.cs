using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishMenuAnimationHandler : MonoBehaviour
{
    [SerializeField] GameObject finishMenu;
    [SerializeField] GameObject timerLine;
    [SerializeField] Animator animator;
    [SerializeField] PauseMenu pauseMenu;

    public void TriggerAnimation()
    {
        animator.Play("PopIn");
    }

    public void DisablePauseMenuActions()
    {
        pauseMenu.Disable();
    }

    public void Appear()
    {
        finishMenu.GetComponent<RectTransform>().localScale = Vector3.one;
        timerLine.GetComponent<RectTransform>().localScale = Vector3.one;
    }

    public void Disappear()
    {
        finishMenu.GetComponent<RectTransform>().localScale = Vector3.zero;
        timerLine.GetComponent<RectTransform>().localScale = Vector3.one;
    }
}
