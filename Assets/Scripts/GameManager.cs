using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int score;

    public bool gamePaused;

    public static GameManager instance;


    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        //Cancel = ESC
        if (Input.GetButtonDown("Cancel"))
        {
            UpdateGamePause();
        }
    }

    private void UpdateGamePause()
    {
        //change to the other state
        gamePaused = !gamePaused;
        //if gamePaused freeze the game else continue normal
        Time.timeScale = (gamePaused) ? 0.0f : 1f;
        //if gamePaused activate the mouse cursor else desactivate
        Cursor.lockState = (gamePaused) ? CursorLockMode.None : CursorLockMode.Locked;

        //Call the HUD method to activate or desactivate the PauseWindow
        HUDController.instance.ChangeStatePauseWindow(gamePaused);


    }

    public void UpdateScore(int points)
    {
        score += points;

        HUDController.instance.UpdateScore(score);
    }
}
