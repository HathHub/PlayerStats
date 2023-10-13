using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Cysharp.Threading.Tasks;
using OpenMod.Unturned.Plugins;
using OpenMod.API.Plugins;
using ShimmyMySherbet.MySQL.EF.Core;
using OpenMod.Core.Plugins;
using System.Management.Instrumentation;
using ShimmyMySherbet.MySQL.EF.Models;
using ShimmyMySherbet.MySQL.EF.Models.TypeModel;
using OpenMod.API.Commands;
using OpenMod.API.Permissions;
using OpenMod.Core.Commands;
using OpenMod.Core.Permissions;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using OpenMod.API.Eventing;
using OpenMod.Core.Eventing;
using OpenMod.Core.Users.Events;
using OpenMod.Core.Commands.Events;
using OpenMod.Core.Users;
using OpenMod.Unturned.Users;
using OpenMod.Unturned.Locations;
using OpenMod.Unturned.Players.Life.Events;
using SDG.Unturned;
using Google.Protobuf.WellKnownTypes;
using System.Collections.Generic;

// For more, visit https://openmod.github.io/openmod-docs/devdoc/guides/getting-started.html

[assembly: PluginMetadata("HathPlayerStats", DisplayName = "HathPlayerStats", Author = "Hath")]
namespace PlayerStats
{

    public class HathPlayerStats : OpenModUnturnedPlugin
    {
        private readonly IConfiguration m_Configuration;
        private readonly IStringLocalizer m_StringLocalizer;
        private readonly ILogger<HathPlayerStats> m_Logger;
        public MySQLEntityClient? Client;
        private readonly IUnturnedUserDirectory m_UnturnedUserDirectory;
        public class StatsDB
        {
            [SQLPrimaryKey]
            public ulong PlayerID;
            public string PlayerName = string.Empty;
            [SQLDefault(0)]
            public int Zombies;
            [SQLDefault(0)]
            public int Messages;
            [SQLDefault(0)]
            public int Deaths;
            [SQLDefault(0)]
            public int Headshots;
            [SQLDefault(0)]
            public int Kills;
            [SQLDefault(0)]
            public int MegaZombies;
            [SQLDefault(0)]
            public int Resources;
            [SQLDefault(0)]
            public int Harvests;
            [SQLDefault(0)]
            public int Fish;
            [SQLDefault(0)]
            public int Animals;
            public DateTime LastUpdated;
        }
        public HathPlayerStats(
            IConfiguration configuration,
            IStringLocalizer stringLocalizer,
            ILogger<HathPlayerStats> logger,
            IUnturnedUserDirectory UnturnedUserDirectory,

            IServiceProvider serviceProvider
            ) : base(serviceProvider)
        {
            m_Configuration = configuration;
            m_StringLocalizer = stringLocalizer;
            m_Logger = logger;
            m_UnturnedUserDirectory = UnturnedUserDirectory;
        }
        protected override async UniTask OnLoadAsync()
        {
            await UniTask.SwitchToThreadPool();
            Client = new MySQLEntityClient(m_Configuration["Database:connectionString"], false);
            if (Client.Connect(out var msg))
            {
                m_Logger.LogInformation("Succesfully connected to database!");
                await Client.CreateTableIfNotExistsAsync<StatsDB>(m_Configuration["Database:TableName"]);
            }
            else
            {
                Logger.LogError($"Failed to connect to database: {msg}");
            }
        }
        protected override async UniTask OnUnloadAsync()
        {
            //  uncomment if you have to access Unturned or UnityEngine APIs
            m_Logger.LogInformation(m_StringLocalizer["plugin_events:plugin_stop"]);
        }
    }
}