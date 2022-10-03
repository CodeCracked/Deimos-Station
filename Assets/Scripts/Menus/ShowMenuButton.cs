using UnityEngine;

public class ShowMenuButton : MonoBehaviour
{
    public GameObject MenuToShow;
    public GameObject MenuToHide;

    public void OnClick()
    {
        if (MenuToShow) MenuToShow.SetActive(true);
        if (MenuToHide) MenuToHide.SetActive(false);
    }
}
