using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    public KeyCode pauseInput;
    public bool isPaused;

    public Transform menu;

    public static PauseController instance;

    private void Awake()
    {
        instance = this;
        isPaused = false;
        menu.gameObject.SetActive(false);
    }
    public void Pause()
    {
        if (Input.GetKeyDown(pauseInput))
        {
            if (!isPaused)
            {
                Debug.Log("Paused");
                Time.timeScale = 0;
                menu.gameObject.SetActive(true);
                isPaused = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Debug.Log("Unpaused");
                Time.timeScale = 1;
                menu.gameObject.SetActive(false);
                isPaused = false;
                Cursor.lockState = CursorLockMode.Locked;
            }

        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Resume()
    {
        menu.gameObject.SetActive(false);
        isPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }

    private void Update()
    {
        Pause();
    }
}
