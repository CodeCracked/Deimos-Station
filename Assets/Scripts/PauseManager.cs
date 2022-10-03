using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public GameObject PauseMenu;
    public GameObject HUD;
    public PlayerMotor PlayerMotor;
    public string PauseButton = "Pause";

    private bool _paused;

    public void Start()
    {
        Unpause(true);
    }
    public void Update()
    {
        if (Input.GetButtonDown(PauseButton))
        {
            if (_paused) Unpause();
            else Pause();
        }
    }

    public void Pause(bool showMenu = true)
    {
        if (!_paused)
        {
            Time.timeScale = 0.0f;
            if (PauseMenu && showMenu) PauseMenu.SetActive(true);
            if (HUD) HUD.SetActive(false);
            if (PlayerMotor)
            {
                PlayerMotor.enabled = false;
                PlayerMotor.LockCursor = false;
            }
            _paused = true;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    public void Unpause(bool force = false)
    {
        if (_paused || force)
        {
            Time.timeScale = 1.0f;
            if (PauseMenu) PauseMenu.SetActive(false);
            if (HUD) HUD.SetActive(true);
            if (PlayerMotor)
            {
                PlayerMotor.enabled = true;
                PlayerMotor.LockCursor = true;
            }
            _paused = false;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
