using UnityEngine;

public class ControlMenu : MonoBehaviour
{
    public void OpenControlMenu()
    {
        gameObject.SetActive(true);
    }

    public void CloseControlMenu()
    {
        gameObject.SetActive(false);
    }
}
