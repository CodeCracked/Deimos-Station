using UnityEngine;

public class ShowMenuButton : MonoBehaviour
{
    public bool Pause = false;
    public bool Unpause = true;
    public GameObject MenuToShow;
    public GameObject MenuToHide;

    public void OnClick()
    {
        if (Pause) Time.timeScale = 0.0f;
        else if (Unpause) Time.timeScale = 1.0f;

        if (MenuToShow) MenuToShow.SetActive(true);
        if (MenuToHide) MenuToHide.SetActive(false);
    }
}
