using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Finish : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] Level level;
    [Header("Objects")]
    [SerializeField] GameObject player;
    [SerializeField] Animator finishAnimator;
    [SerializeField] FinishMenuAnimationHandler menuAnimation;
    [SerializeField] Timer timer;
    [SerializeField] Countdown countdown;
    [SerializeField] PlayerLook cameraMovement;
    [Header("Particles")]
    [SerializeField] ParticleSystem sparkles;
    [SerializeField] ParticleSystem explosion;

    private void OnTriggerEnter(Collider other)
    {
        if (other.Equals(player.GetComponent<Collider>()))
        {
            finishAnimator.Play("Collect");
            player.GetComponent<Controls>().enabled = false;
        }
    }

    public void PlayMenuAnimation()
    {
        menuAnimation.TriggerAnimation();
        // Enable mouse
        Cursor.lockState = CursorLockMode.None;
        cameraMovement.MenuOpen = true;
    }

    public void StopSparkles()
    {
        sparkles.Stop();
    }

    public void ExplodeEffect()
    {
        explosion.Play();
        FinishLevel();
    }

    private void FinishLevel()
    {
        timer.StopTimer();
        timer.SaveTimer(level);
    }
}
