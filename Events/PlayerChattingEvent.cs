using Microsoft.Extensions.Logging;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using OpenMod.API.Eventing;
using OpenMod.Unturned.Users;
using OpenMod.Unturned.Players.Life.Events;
using PlayerStats;
using System.Text;
using SDG.Unturned;
using OpenMod.Unturned.Players.Chat.Events;

// For more, visit https://openmod.github.io/openmod-docs/devdoc/guides/getting-started.html

public class PlayerChattingEvent : IEventListener<UnturnedPlayerChattingEvent>
{
    private readonly IUnturnedUserDirectory m_UnturnedUserDirectory;
    private readonly ILogger<PlayerChattingEvent> m_Logger;
    private HathPlayerStats Plugin { get; }
    public PlayerChattingEvent(
        IUnturnedUserDirectory unturnedUserDirectory,
         HathPlayerStats plugin,
        ILogger<PlayerChattingEvent> logger
        )
    {
        m_UnturnedUserDirectory = unturnedUserDirectory;
        m_Logger = logger;
        Plugin = plugin;
    }
    public async Task HandleEventAsync(object? sender, UnturnedPlayerChattingEvent @event )
    {
        await UniTask.SwitchToThreadPool();
        await Plugin.Client!.ExecuteNonQueryAsync($"INSERT INTO HathPlayerStats (PlayerID, PlayerName, Messages, LastUpdated) VALUES(@0, @1, 1, NOW()) ON DUPLICATE KEY UPDATE PlayerName = @1, Messages = Messages + 1, LastUpdated = NOW();", @event.Player.SteamId.ToString(), Encoding.UTF8.GetString(Encoding.GetEncoding("ISO-8859-1").GetBytes(@event.Player.Player.name)));
    }
}