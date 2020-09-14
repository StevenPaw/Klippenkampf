using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SP_PlayerInterfaceController : MonoBehaviour
{
    public bool MenuOpen = false;
    [SerializeField] Canvas pauseMenu = null;

    private const string PlayerPrefsFullscreenKey = "false";

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MenuOpen = !MenuOpen;
        }

        if (MenuOpen)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            pauseMenu.gameObject.SetActive(true);
            
        } else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            pauseMenu.gameObject.SetActive(false);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting");
    }

    public void ChangeFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
}

