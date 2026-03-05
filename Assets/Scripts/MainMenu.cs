using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        GameManager.Instance.GotoGame();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
