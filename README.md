# PlayerStats
PlayerStats is a plugin for OpenMod. It enriches your server by saving and displaying player statistics.  a range of in-game performance metrics, and other pertinent data.
### Downloading/Updating
Run on console `openmod install Hath.PlayerStats` after [installing OpenMod](https://openmod.github.io/openmod-docs/userdoc/installation/unturned.html)
### Configuration
```ruby
# See https://github.com/HathHub/PlayerStats
Database:
    connectionString: "Server=localhost;Port=3306;Database=dbname;User=username;Password=password;"
Messages:
    Stats: "[Stats for {PlayerName} #{Position}]\n Kills: {Kills} Deaths: {Deaths} Headshots: {Headshots} Accuracy: {Accuracy}\n Messages: {Messages} Zombies: {Zombies} Megas: {MegaZombies}\nFish: {Fish} Animals {Animals} Harvests: {Harvests} Resources {Resources}"
    RankingHeader: "<b>[Top 5 players]</b>"
    Ranking: "#{Position} {PlayerName}: Kills: {Kills} Deaths: {Deaths} Headshots: {Headshots} Accuracy: {Accuracy}" 
Stats:
    sortBy: "Kills"
```
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

