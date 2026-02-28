using System;
using Unity.VisualScripting;

public static class GameEvents 
{
    public static Action<SwipeDirection,CardSO> CardSwiped;
    public static Action<bool> GameOver;
}
