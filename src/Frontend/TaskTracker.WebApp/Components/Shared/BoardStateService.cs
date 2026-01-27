namespace TaskTracker.WebApp.Components.Shared;

public class BoardStateService
{
    public event Action OnBoardListChanged;

    public event Action OnBoardSettingsChanged;

    public void NotifyBoardListChanged()
    {
        OnBoardListChanged?.Invoke();
    }

    public void NotifyBoardSettingsChanged()
    {
        OnBoardSettingsChanged?.Invoke();
    }
}
