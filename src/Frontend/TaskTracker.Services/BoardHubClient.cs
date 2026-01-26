using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.JSInterop;
using TaskTracker.Domain.DTOs.Columns;
using TaskTracker.Domain.DTOs.Tasks;
using TaskTracker.Services.Abstraction.Interfaces.Services;

namespace TaskTracker.Services;

public class BoardHubClient : IBoardHubClient
{
    private HubConnection? _hubConnection;
    private readonly IConfiguration _configuration;
    private readonly ISessionCacheService _sessionCacheService;

    public event Action<int>? OnBoardChanged;

    public event Action<int, TaskSummaryDto>? OnTaskCreated;
    public event Action<int, TaskSummaryDto>? OnTaskUpdated;
    public event Action<int, int>? OnTaskDeleted;

    public event Action<int, ColumnSummaryDto>? OnColumnCreated;
    public event Action<int, ColumnSummaryDto>? OnColumnUpdated;
    public event Action<int, int>? OnColumnDeleted;

    public event Action<int, IReadOnlyCollection<int>>? OnOnlineUsersUpdated;

    public BoardHubClient(
        IConfiguration configuration,
        ISessionCacheService sessionCacheService)
    {
        _configuration = configuration;
        _sessionCacheService = sessionCacheService;
    }

    public async Task ConnectAsync()
    {
        if (_hubConnection is not null && _hubConnection.State == HubConnectionState.Connected)
            return;

        var apiUrl = _configuration["ApiSettings:BaseUrl"];
        var hubUrl = $"{apiUrl}/hubs/board";

        _hubConnection = new HubConnectionBuilder()
            .WithUrl(hubUrl, options =>
            {
                options.AccessTokenProvider = async () =>
                {
                    var sid = _sessionCacheService.CurrentSessionId;
                    return _sessionCacheService.GetSessionData(sid).AccessToken;
                };
            })
            .WithAutomaticReconnect()
            .Build();

        RegisterReceivers(_hubConnection);

        try
        {
            await _hubConnection.StartAsync();
            Console.WriteLine("SignalR Connected!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"SignalR Connection Error: {ex.Message}");
        }
    }

    public async Task JoinBoardGroupAsync(int boardId, int userId)
    {
        if (_hubConnection is not null && _hubConnection.State == HubConnectionState.Connected)
        {
            await _hubConnection.InvokeAsync("JoinBoard", boardId, userId);
        }
    }

    public async Task LeaveBoardGroupAsync(int boardId, int userId)
    {
        if (_hubConnection is not null && _hubConnection.State == HubConnectionState.Connected)
        {
            await _hubConnection.InvokeAsync("LeaveBoard", boardId, userId);
        }
    }

    public async Task DisconnectAsync()
    {
        if (_hubConnection is not null)
        {
            await _hubConnection.StopAsync();
            await _hubConnection.DisposeAsync();
            _hubConnection = null;
        }
    }

    public async ValueTask DisposeAsync()
    {
        await DisconnectAsync();
    }

    private void RegisterReceivers(HubConnection connection)
    {
        connection.On<int, IReadOnlyCollection<int>>("ReceiveOnlineUsersUpdated", (boardId, users) =>
        {
            OnOnlineUsersUpdated?.Invoke(boardId, users);
        });

        connection.On<int, TaskSummaryDto>("ReceiveTaskCreated", (boardId, task) =>
        {
            OnTaskCreated?.Invoke(boardId, task);
        });

        connection.On<int, TaskSummaryDto>("ReceiveTaskUpdated", (boardId, task) =>
        {
            OnTaskUpdated?.Invoke(boardId, task);
        });

        connection.On<int, int>("ReceiveTaskDeleted", (boardId, taskId) =>
        {
            OnTaskDeleted?.Invoke(boardId, taskId);
        });

        connection.On<int>("ReceiveBoardChanged", boardId =>
        {
            OnBoardChanged?.Invoke(boardId);
        });

        connection.On<int, ColumnSummaryDto>("ReceiveColumnCreated", (boardId, column) =>
        {
            OnColumnCreated?.Invoke(boardId, column);
        });

        connection.On<int, ColumnSummaryDto>("ReceiveColumnUpdated", (boardId, column) =>
        {
            OnColumnUpdated?.Invoke(boardId, column);
        });

        connection.On<int, int>("ReceiveColumnDeleted", (boardId, columnId) =>
        {
            OnColumnDeleted?.Invoke(boardId, columnId);
        });
    }
}