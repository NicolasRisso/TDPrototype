using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public void Pause(bool pause)
    {
        if (pause){
            gameObject.SetActive(true);
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            gameObject.SetActive(false);
            Time.timeScale = 1;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Game Quit");
    }
}
