using System;
using Unity.VisualScripting;

public static class GameEvents 
{
    public static Action<SwipeDirection,CardSO> CardSwiped;
    public static Action<bool> GameOver;
    public static Action<StatType, float> StatChanged;
    public static Action ImprisonButtonClicked;
    public static Action GameStarted;
    public static System.Action<int> CancerStageChanged;

}
