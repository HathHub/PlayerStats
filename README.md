# PlayerStats
PlayerStats is a plugin for OpenMod. It enriches your server by saving and displaying player statistics.  a range of in-game performance metrics, and other pertinent data.
<pre>
```yaml
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
</pre>
