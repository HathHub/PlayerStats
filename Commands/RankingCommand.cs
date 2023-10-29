
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
using System.Text;
using Cysharp.Threading.Tasks;

namespace PlayerStats.Commands
{
    [Command("ranking")]
    [CommandAlias("r")]
    [CommandDescription("Retrieves server ranking.")]
    public class CommandRanking : Command
    {
        private HathPlayerStats Plugin { get; }
        private readonly IConfiguration m_Configuration;
        private readonly ILogger<CommandRanking> m_Logger;
        private readonly IUnturnedUserDirectory m_UnturnedUserDirectory;
        public CommandRanking(IServiceProvider serviceProvider, HathPlayerStats plugin, IUnturnedUserDirectory unturnedUserDirectory, ILogger<CommandRanking> logger, IConfiguration configuration) : base(serviceProvider)
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
            List<StatsDBRanked> stats = await Plugin.Client!.QueryAsync<StatsDBRanked>("SELECT *, ROW_NUMBER() OVER (ORDER BY 'Kills' DESC) AS 'Rank' FROM HathPlayerStats ORDER BY @0 DESC LIMIT @1", m_Configuration["Stats:SortBy"], int.Parse(m_Configuration["Stats:Limit"]));
            if (stats is null) throw new UserFriendlyException("Stats couldnt be get!");
            string MessagePlaceholder = m_Configuration["Messages:Ranking"];
            StringBuilder Message = new StringBuilder();
            Message.AppendLine(m_Configuration["Messages:RankingHeader"]);
            foreach (var stat in stats)
            {
                Message.AppendLine(Smart.Format("\n" + MessagePlaceholder, new
                {
                    PlayerName = stat.PlayerName,
                    Kills = stat.Kills,
                    Deaths = stat.Deaths,
                    Messages = stat.Messages,
                    Kdr = (!double.IsNaN((double)stat.Kills / stat.Deaths) && stat.Deaths != 0) ? ((double)stat.Kills / stat.Deaths).ToString("F2") : "0.0",
                    Headshots = stat.Headshots,
                    Zombies = stat.Zombies,
                    MegaZombies = stat.MegaZombies,
                    Resources = stat.Resources,
                    Harvests = stat.Harvests,
                    Fish = stat.Fish,
                    Animals = stat.Animals,
                    Position = stat.Rank,
                    Accuracy = (stat.Headshots == 0 || stat.Kills == 0) ? "0.0" : ((double)stat.Headshots / stat.Kills * 100).ToString("F2")
                }));
            }

            await Context.Actor.PrintMessageAsync(Message.ToString());
        }
    }
}
