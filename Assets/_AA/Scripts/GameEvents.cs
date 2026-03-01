using System;

public static class GameEvents
{
    public static Action<SwipeDirection, CardSO> CardSwiped;

    // DEGISTIRILDI: Artik sadece kazanma durumunu degil, ayni zamanda oyun sonu hikayesini de tasiyor
    public static Action<bool, string> GameOver;

    public static Action<StatType, float> StatChanged;
    public static Action ImprisonButtonClicked;
    public static Action GameStarted;
    public static Action<int> CancerStageChanged; 


}
