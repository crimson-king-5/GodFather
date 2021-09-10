using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    public KeyCode pauseInput;
    public bool isPaused;

    public Transform menu;

    public static PauseController instance;

    private FirstPersonMovement _fpm;

    private void Awake()
    {
        _fpm = FindObjectOfType<FirstPersonMovement>();
        instance = this;
        isPaused = false;
        menu.gameObject.SetActive(false);
    }
    public void Pause()
    {
        Debug.Log("Paused + NO MORE RADIAL MENU");
        SelectionRadialMenu.instance.radialMenuHolder.gameObject.SetActive(false);
        Time.timeScale = 0;
        menu.gameObject.SetActive(true);
        isPaused = true;
        Cursor.lockState = CursorLockMode.None;
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
        if (isPaused)
            SelectionRadialMenu.instance.radialMenuHolder.gameObject.SetActive(false);

        if (Input.GetKeyDown(pauseInput) && !SelectionRadialMenu.instance.menuEnable)
        {
            if (!isPaused)
            {
                //Debug.Log("Paused");
                Time.timeScale = 0;
                menu.gameObject.SetActive(true);
                isPaused = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                //Debug.Log("Unpaused");
                Time.timeScale = 1;
                menu.gameObject.SetActive(false);
                isPaused = false;
                Cursor.lockState = CursorLockMode.Locked;
            }

        }

        if (!isPaused)
        {
            if (SelectionRadialMenu.instance.menuEnable)
                Cursor.lockState = CursorLockMode.None;
            else
            {
                _fpm.canRot = true;
                Cursor.lockState = CursorLockMode.Locked;
            }

        }
    }
}
