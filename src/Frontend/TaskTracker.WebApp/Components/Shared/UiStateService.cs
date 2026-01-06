namespace TaskTracker.WebApp.Components.Shared;

public class UiStateService
{
    public event Action OnBoardListChanged;

    public void NotifyBoardListChanged()
    {
        OnBoardListChanged?.Invoke();
    }
}
