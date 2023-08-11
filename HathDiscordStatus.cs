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
using ShimmyMySherbet.DiscordWebhooks;
using ShimmyMySherbet.DiscordWebhooks.Helpers;
using ShimmyMySherbet.DiscordWebhooks.Models;
using OpenMod.Unturned.Users;
using Org.BouncyCastle.Crmf;
using SDG.Unturned;
using System.Collections.Generic;
using OpenMod.API.Eventing;
using OpenMod.Core.Eventing;
using OpenMod.Core.Users.Events;
using OpenMod.Core.Helpers;

// For more, visit https://openmod.github.io/openmod-docs/devdoc/guides/getting-started.html

[assembly: PluginMetadata("HathDiscordStatus", DisplayName = "Hath Discord Status", Author = "Hath")]
namespace VirtualGarage
{

    public class HathDiscordStatus : OpenModUnturnedPlugin
    {
        private readonly IConfiguration m_Configuration;
        private readonly IStringLocalizer m_StringLocalizer;
        private readonly ILogger<HathDiscordStatus> m_Logger;
        private readonly IUnturnedUserDirectory m_UnturnedUserDirectory;
        private Dictionary<string, DateTime> PlayerJoins = new Dictionary<string, DateTime>();
        private bool _ShouldSend = false;

        public HathDiscordStatus(
            IConfiguration configuration,
            IStringLocalizer stringLocalizer,
            ILogger<HathDiscordStatus> logger,
            IUnturnedUserDirectory unturnedUsers,
            IServiceProvider serviceProvider
            ) : base(serviceProvider)
        {
            m_Configuration = configuration;
            m_StringLocalizer = stringLocalizer;
            m_Logger = logger;
            m_UnturnedUserDirectory = unturnedUsers;
        }

        protected override async UniTask OnLoadAsync()
        {
            _ShouldSend = true;
            AsyncHelper.Schedule("My Task", () => MyPeriodicTask());

        }

        public async Task MyPeriodicTask()
        {
            while (_ShouldSend) // ensure this task runs only as long as the plugin is loaded 
            {
                await UniTask.SwitchToThreadPool();
                var embed = new WebhookMessage()
                            .PassEmbed()
                                .WithColor(EmbedColor.Purple)
                                .WithAuthor("[TX] UnturnedLive -> BattleEye+ | Kits | Home", "https://media.discordapp.net/attachments/1134313985525682307/1134716021488496640/IMG_20230729_011901_721.png")
                            .Finalize();
                embed.Embeds[0].Fields.Add(new WebhookField() { Inline = false, Name = $"IP: Unturned.Live   Port:  27015   :map: Arid  :busts_in_silhouette: {Provider.clients.Count.ToString()}/100", Value = "Unete ya! Qué esperas?" });
                foreach (var user in m_UnturnedUserDirectory.GetOnlineUsers())
                {
                    TimeSpan timeSpan = TimeSpan.FromSeconds(user.Player.SteamPlayer.realtimeSinceFirstPingRequest);
                    string formattedTime;

                    if (timeSpan.TotalHours >= 1)
                    {
                        formattedTime = $"{(int)timeSpan.TotalHours}h {(int)timeSpan.Minutes}m {timeSpan.Seconds}s";
                    }
                    else if (timeSpan.TotalMinutes >= 1)
                    {
                        formattedTime = $"{(int)timeSpan.TotalMinutes}m {timeSpan.Seconds}s";
                    }
                    else
                    {
                        formattedTime = $"{timeSpan.Seconds}s";
                    }
                    embed.Embeds[0].Fields.Add(new WebhookField() { Inline = true, Name = " ", Value = $"👤 [**{user.DisplayName}**](https://steamcommunity.com/profiles/{user.SteamId})\r\n> ⌛ {formattedTime}\r\n> 🏓 {(int)(user.Player.SteamPlayer.ping * 1000f)}ms" });
                }
                var toedit = new PostedDiscordMessage()
                    .MessageID = 1139070640247275550;
                await DiscordWebhookService.EditMessageAsync("https://discord.com/api/webhooks/1136076611981148160/3XMKk8fl8EZtCL0XIK3_H8c5K7z--gZLZLxGkmPWiV9affWpAWpfwcUZo9jDvlIj3PVC", embed, 1139070640247275550);
                await Task.Delay(TimeSpan.FromSeconds(10));
            }
        }


        protected override async UniTask OnUnloadAsync()
        {
            _ShouldSend = false;
            m_Logger.LogInformation(m_StringLocalizer["plugin_events:plugin_stop"]);
        }
    }
}
