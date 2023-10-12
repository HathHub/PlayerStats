using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using OpenMod.API.Eventing;
using OpenMod.Unturned.Users;
using OpenMod.Unturned.Locations;
using OpenMod.Unturned.Players.Life.Events;
using PlayerStats;
using System.Text;
using OpenMod.Unturned.Zombies.Events;
using OpenMod.Unturned.Players.Stats.Events;
using SDG.Unturned;

// For more, visit https://openmod.github.io/openmod-docs/devdoc/guides/getting-started.html

public class StatIncremented : IEventListener<UnturnedPlayerStatIncrementedEvent>
{
    private readonly IUnturnedUserDirectory m_UnturnedUserDirectory;
    private readonly IUnturnedLocationDirectory m_UnturnedLocationDirectory;
    private readonly IStringLocalizer m_StringLocalizer;
    private readonly IConfiguration m_Configuration;
    private readonly ILogger<StatIncremented> m_Logger;
    private HathPlayerStats Plugin { get; }
    public StatIncremented(
        IUnturnedUserDirectory unturnedUserDirectory,
        IUnturnedLocationDirectory unturnedLocationDirectory,
        IStringLocalizer stringLocalizer,
        IConfiguration configuration,
         HathPlayerStats plugin,
        ILogger<StatIncremented> logger
        )
    {
        m_UnturnedUserDirectory = unturnedUserDirectory;
        m_UnturnedLocationDirectory = unturnedLocationDirectory;
        m_StringLocalizer = stringLocalizer;
        m_Configuration = configuration;
        m_Logger = logger;
        Plugin = plugin;
    }
    public async Task HandleEventAsync(object? sender, UnturnedPlayerStatIncrementedEvent @event )
    {
        await UniTask.SwitchToThreadPool();
        switch (@event.Stat)
        {
            case EPlayerStat.FOUND_FISHES:
                await Plugin.Client!.ExecuteNonQueryAsync($"INSERT INTO HathPlayerStats (PlayerID, PlayerName, Fish, LastUpdated) VALUES(@0, @1, 1, NOW()) ON DUPLICATE KEY UPDATE PlayerName = @1, Fish = Fish + 1, LastUpdated = NOW();", @event.Player.SteamId.ToString(), Encoding.UTF8.GetString(Encoding.GetEncoding("ISO-8859-1").GetBytes(@event.Player.Player.name)));
            break;
            case EPlayerStat.KILLS_ANIMALS:
            await Plugin.Client!.ExecuteNonQueryAsync($"INSERT INTO HathPlayerStats (PlayerID, PlayerName, Animals, LastUpdated) VALUES(@0, @1, 1, NOW()) ON DUPLICATE KEY UPDATE PlayerName = @1, Animals = Animals + 1, LastUpdated = NOW();", @event.Player.SteamId.ToString(), Encoding.UTF8.GetString(Encoding.GetEncoding("ISO-8859-1").GetBytes(@event.Player.Player.name)));
            break;
            case EPlayerStat.FOUND_RESOURCES:
            await Plugin.Client!.ExecuteNonQueryAsync($"INSERT INTO HathPlayerStats (PlayerID, PlayerName, Resources, LastUpdated) VALUES(@0, @1, 1, NOW()) ON DUPLICATE KEY UPDATE PlayerName = @1, Resources = Resources + 1, LastUpdated = NOW();", @event.Player.SteamId.ToString(), Encoding.UTF8.GetString(Encoding.GetEncoding("ISO-8859-1").GetBytes(@event.Player.Player.name)));
            break;
            case EPlayerStat.KILLS_ZOMBIES_NORMAL:
            await Plugin.Client!.ExecuteNonQueryAsync($"INSERT INTO HathPlayerStats (PlayerID, PlayerName, Zombies, LastUpdated) VALUES(@0, @1, 1, NOW()) ON DUPLICATE KEY UPDATE PlayerName = @1, Zombies = Zombies + 1, LastUpdated = NOW();", @event.Player.SteamId.ToString(), Encoding.UTF8.GetString(Encoding.GetEncoding("ISO-8859-1").GetBytes(@event.Player.Player.name)));
            break;
            case EPlayerStat.KILLS_ZOMBIES_MEGA:
            await Plugin.Client!.ExecuteNonQueryAsync($"INSERT INTO HathPlayerStats (PlayerID, PlayerName, MegaZombies, LastUpdated) VALUES(@0, @1, 1, NOW()) ON DUPLICATE KEY UPDATE PlayerName = @1, MegaZombies = MegaZombies + 1, LastUpdated = NOW();", @event.Player.SteamId.ToString(), Encoding.UTF8.GetString(Encoding.GetEncoding("ISO-8859-1").GetBytes(@event.Player.Player.name)));
            break;
            case EPlayerStat.FOUND_PLANTS:
            await Plugin.Client!.ExecuteNonQueryAsync($"INSERT INTO HathPlayerStats (PlayerID, PlayerName, Harvests, LastUpdated) VALUES(@0, @1, 1, NOW()) ON DUPLICATE KEY UPDATE PlayerName = @1, Harvests = Harvests + 1, LastUpdated = NOW();", @event.Player.SteamId.ToString(), Encoding.UTF8.GetString(Encoding.GetEncoding("ISO-8859-1").GetBytes(@event.Player.Player.name)));
            break;
        }
    }
}