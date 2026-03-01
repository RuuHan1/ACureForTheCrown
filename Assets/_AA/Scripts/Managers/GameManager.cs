using UnityEngine;

public class GameManager : MonoBehaviour
{

    private void Awake()
    {
        Time.timeScale = 0f;
    }

    public void OnPlayButtonClicked()
    {
        Time.timeScale = 1f;
        GameEvents.GameStarted?.Invoke();
    }
}
