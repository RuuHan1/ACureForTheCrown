using System;
using Unity.VisualScripting;

public static class GameEvents 
{
    public static Action<bool,CardSO> CardSwiped;
    public static Action<bool> GameOver;
}
