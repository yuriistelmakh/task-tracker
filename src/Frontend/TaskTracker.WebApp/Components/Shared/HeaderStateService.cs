namespace TaskTracker.WebApp.Components.Shared;

public class HeaderStateService
{
    public event Action OnUserUpdated;

    public void NotifyUserUpdated()
    {
        OnUserUpdated?.Invoke();
    }
}
