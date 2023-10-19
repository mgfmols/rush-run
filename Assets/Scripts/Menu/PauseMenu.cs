using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Mechanics")]
    [SerializeField] Timer timer;
    [SerializeField] Countdown countdown;
    [SerializeField] PlayerLook cameraMovement;
    [SerializeField] Rigidbody player;
    [SerializeField] Controls playerControls;
    [Header("UI")]
    [SerializeField] GameObject pauseMenu;
    [SerializeField] Animator pauseMenuAnimator;
    [Header("Sounds")]
    [SerializeField] List<EventReference> soundEvents;
    [SerializeField] EffectSounds effectSounds;

    private bool isPaused;

    public void OpenMenu()
    {
        // Enable pause menu overlay
        pauseMenuAnimator.Play("PopIn");
        // Disable physics
        if (!isPaused)
        {
            Time.timeScale = 0;
        }
        // Stop timer
        timer.StopTimer();
        // Enable mouse
        Cursor.lockState = CursorLockMode.None;
        cameraMovement.MenuOpen = true;
        // Disable speed sounds
        effectSounds.Pause();
        countdown.Cancel();

        isPaused = true;
    }

    public void CloseMenu()
    {
        // Disable pause menu overlay
        pauseMenuAnimator.Play("PopOut");
        // Enable physics
        if (isPaused)
        {
            countdown.ResetCountdown();
        }
        // Disable mouse
        Cursor.lockState = CursorLockMode.Locked;
        cameraMovement.MenuOpen = false;
        // Enable speed sounds
        effectSounds.UnPause();

        isPaused = false;
    }

    public void Disable()
    {
        countdown.Disable();
        cameraMovement.MenuOpen = true;
        playerControls.PauseMenuEnabled = false;
    }

    public void Enable()
    {
        cameraMovement.MenuOpen = false;
        playerControls.PauseMenuEnabled = true;
    }

    public void RetryLevel()
    {
        ResetGravity();
        SceneManager.LoadScene(1);
    }

    private void ResetGravity()
    {
        if (Physics.gravity.y > -19)
        {
            Physics.gravity = new Vector3(0, -19.62f, 0);
        }
    }

    public void ExitLevel()
    {
        ResetGravity();
        StopSounds();
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void StopSounds()
    {
        RuntimeManager.MuteAllEvents(true);
    }
}
