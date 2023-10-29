using Microsoft.Extensions.Logging;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using OpenMod.API.Eventing;
using OpenMod.Unturned.Users;
using OpenMod.Unturned.Players.Life.Events;
using PlayerStats;
using System.Text;
using SDG.Unturned;

// For more, visit https://openmod.github.io/openmod-docs/devdoc/guides/getting-started.html

public class PlayerDeathEvent : IEventListener<UnturnedPlayerDeathEvent>
{
    private readonly IUnturnedUserDirectory m_UnturnedUserDirectory;
    private readonly ILogger<PlayerDeathEvent> m_Logger;
    private HathPlayerStats Plugin { get; }
    public PlayerDeathEvent(
        IUnturnedUserDirectory unturnedUserDirectory,
         HathPlayerStats plugin,
        ILogger<PlayerDeathEvent> logger
        )
    {
        m_UnturnedUserDirectory = unturnedUserDirectory;
        m_Logger = logger;
        Plugin = plugin;
    }
    public async Task HandleEventAsync(object? sender, UnturnedPlayerDeathEvent @event )
    {
        
        UnturnedUser? killer = m_UnturnedUserDirectory.FindUser(@event.Instigator);
        await UniTask.SwitchToThreadPool();
        await Plugin.Client!.ExecuteNonQueryAsync($"INSERT INTO HathPlayerStats (PlayerID, PlayerName, Deaths, LastUpdated) VALUES(@0, @1, 1, NOW()) ON DUPLICATE KEY UPDATE PlayerName = @1, Deaths = Deaths + 1, LastUpdated = NOW();", @event.Player.SteamId.ToString(), Encoding.UTF8.GetString(Encoding.GetEncoding("ISO-8859-1").GetBytes(@event.Player.Player.name)));
        if (@event.Instigator != @event.Player.SteamId && killer is not null)
        {
            await Plugin.Client!.ExecuteNonQueryAsync($"INSERT INTO HathPlayerStats (PlayerID, PlayerName, Kills, LastUpdated) VALUES(@0, @1, 1, NOW()) ON DUPLICATE KEY UPDATE PlayerName = @1, Kills = Kills + 1,  Headshots = CASE WHEN @2 = 'SKULL' THEN Headshots +1 ELSE Headshots END, LastUpdated = NOW();", killer!.Player.SteamId.ToString(), Encoding.UTF8.GetString(Encoding.GetEncoding("ISO-8859-1").GetBytes(killer!.Player.Player.name)), @event.Limb.ToString());
        }
    }
}