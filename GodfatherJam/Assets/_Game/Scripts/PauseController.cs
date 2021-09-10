using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    public KeyCode pauseInput;
    private bool isPaused;

    private void Awake()
    {
        isPaused = false;
    }
    public void Pause()
    {

    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Resume()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(pauseInput))
        {

        }
    }
}
