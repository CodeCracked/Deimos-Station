using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject PauseMenu;
    public PlayerMotor PlayerMotor;
    public string PauseButton = "Pause";

    private bool _paused;

    public void Update()
    {
        if (Input.GetButtonDown(PauseButton))
        {
            if (_paused) Unpause();
            else Pause();
        }
    }

    public void Pause()
    {
        if (!_paused)
        {
            Time.timeScale = 0.0f;
            if (PauseMenu) PauseMenu.SetActive(true);
            if (PlayerMotor) PlayerMotor.enabled = false;
            _paused = true;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    public void Unpause()
    {
        if (_paused)
        {
            Time.timeScale = 1.0f;
            if (PauseMenu) PauseMenu.SetActive(false);
            if (PlayerMotor) PlayerMotor.enabled = true;
            _paused = false;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
