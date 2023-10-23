# PlayerStats
[![Nuget](https://img.shields.io/nuget/v/Hath.PlayerStats)](https://www.nuget.org/packages/Hath.PlayerStats/)
[![Nuget](https://img.shields.io/nuget/dt/Hath.PlayerStats?label=nuget%20downloads)](https://www.nuget.org/packages/Hath.PlayerStats/)

PlayerStats is a plugin for OpenMod. It enriches your server by saving and displaying player statistics.  a range of in-game performance metrics, and other pertinent data.
### Downloading/Updating
Run on console `openmod install Hath.PlayerStats` after [installing OpenMod](https://openmod.github.io/openmod-docs/userdoc/installation/unturned.html)
### Configuration
```ruby
# See https://github.com/HathHub/PlayerStats

Database:
  ConnectionString: "Server=localhost;Port=3306;Database=dbname;User=username;Password=password;"
  TableName: "HathPlayerStats"

Messages:
  Stats: "[Stats for {PlayerName} #{Position}]\n Kills: {Kills} Deaths: {Deaths} Headshots: {Headshots} Accuracy: {Accuracy}\n Messages: {Messages} Zombies: {Zombies} Megas: {MegaZombies}\nFish: {Fish} Animals: {Animals} Harvests: {Harvests} Resources: {Resources}"
  RankingHeader: "<b>[Top 5 players]</b>"
  Ranking: "#{Position} {PlayerName}: Kills: {Kills} Deaths: {Deaths} Headshots: {Headshots} Accuracy: {Accuracy}"

Stats:
  SortBy: "Kills"
  Limit: "5"

Discord:
  Enabled: true    
  WebhookURL: ""
  Embed:
    Top: 5
    SortBy: "Accurracy"

Rewards:
  - Variable: "Kills"
    Threshold: 100
    Prizes:
      - "give {PlayerID} 363"
      - "give {PlayerID} 17"
    Global: false
    Messages:
      - "Congratulations! You've reached 100 kills and earned a Maplestrike and Drum."

  - Variable: "Headshots"
    Threshold: 50
    Prizes:
      - "give {PlayerID} 363"
      - "give {PlayerID} 17"
    Global: true
    Messages:
      - "Sharpshooter Alert! {PlayerName} achieved 50 headshots and earned a Maplestrike. Everyone, cheer!"

```
- `{PlayerID}`: Represents the player's SteamID64.
- `{PlayerName}`: Represents the player's name in various messages.
- `{Position}`: Represents the player's position in the ranking.
- `{Kills}`: Represents the number of kills a player has.
- `{Deaths}`: Represents the number of deaths a player has.
- `{Headshots}`: Represents the number of headshots a player has.
- `{Accuracy}`: Represents the accuracy of a player.
- `{Messages}`: Represents the number of messages sent by a player.
- `{Zombies}`: Represents the number of zombies killed by a player.
- `{MegaZombies}`: Represents the number of mega zombies killed by a player.
- `{Fish}`: Represents the number of fish caught by a player.
- `{Animals}`: Represents the number of animals killed by a player.
- `{Harvests}`: Represents the number of harvests performed by a player.
- `{Resources}`: Represents the number of resources gathered by a player.

### Plugin Commands:

1. **/ranking**
   - Description: View the top players on the server.
   - Permissions: `HathPlayerStats:commands.ranking`

2. **/stats [playername]**
   - Description: View detailed statistics for a player (optional playername parameter).
   - Permissions: `HathPlayerStats:commands.stats`

## Permissions:

- `HathPlayerStats:commands.ranking`: Allows access to the /ranking command.
- `HathPlayerStats:commands.stats`: Allows access to the /stats command.

