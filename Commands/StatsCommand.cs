
using Microsoft.Extensions.Logging;
using NuGet.Protocol.Plugins;
using OpenMod.API.Commands;
using OpenMod.API.Users;
using OpenMod.Core.Commands;
using OpenMod.Core.Users;
using OpenMod.Unturned.Users;
using System;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using PlayerStats;
using static PlayerStats.HathPlayerStats;
using Command = OpenMod.Core.Commands.Command;
using JetBrains.Annotations;
using MoreLinq.Extensions;
using System.Collections.Generic;
using SDG.Unturned;
using Microsoft.Extensions.Configuration;
using NuGet.Protocol;
using SmartFormat;
using static UnityEngine.Random;
using Cysharp.Threading.Tasks;

namespace PlayerStats.Commands
{
    [Command("stats")]
    [CommandAlias("s")]
    [CommandDescription("Retrieves players stats.")]
    public class CommandStats : Command
    {
        private HathPlayerStats Plugin { get; }
        private readonly IConfiguration m_Configuration;
        private readonly ILogger<CommandStats> m_Logger;
        private readonly IUnturnedUserDirectory m_UnturnedUserDirectory;
        public CommandStats(IServiceProvider serviceProvider, HathPlayerStats plugin, IUnturnedUserDirectory unturnedUserDirectory, ILogger<CommandStats> logger, IConfiguration configuration) : base(serviceProvider)
        {
            Plugin = plugin;
            m_Configuration = configuration;
            m_UnturnedUserDirectory = unturnedUserDirectory;
            m_Logger = logger;
        }
        public class StatsDBRanked : Stats
        {
            public int Rank { get; set; }
        }
        protected override async Task OnExecuteAsync()
        {
            await UniTask.SwitchToThreadPool();
            StatsDBRanked stats = await Plugin.Client!.QuerySingleAsync<StatsDBRanked>("SELECT *, ROW_NUMBER() OVER (ORDER BY 'Kills' DESC) AS 'Rank' FROM HathPlayerStats WHERE PlayerID = @0 ORDER BY @1 DESC", Context.Actor.Id, m_Configuration["Stats:SortBy"]);
            if (stats is null) throw new UserFriendlyException("Stats couldnt be get!");
            string message = m_Configuration["Messages:Stats"];
            var result = Smart.Format(message, new
            {
                PlayerName = stats.PlayerName,
                Kills = stats.Kills,
                Deaths = stats.Deaths,
                Messages = stats.Messages,
                Kdr = (!double.IsNaN((double)stats.Kills / stats.Deaths) && stats.Deaths != 0) ? ((double)stats.Kills / stats.Deaths).ToString("F2") : "0.0",
                Headshots = stats.Headshots,
                Zombies = stats.Zombies,
                MegaZombies = stats.MegaZombies,
                Resources = stats.Resources,
                Harvests = stats.Harvests,
                Fish = stats.Fish,
                Animals = stats.Animals,
                Position = stats.Rank,
                Accuracy = (stats.Headshots == 0 || stats.Kills == 0) ? "0.0" : ((double)stats.Headshots / stats.Kills * 100).ToString("F2")
            });
            await Context.Actor.PrintMessageAsync(result);
        }
    }
}
