using ShimmyMySherbet.MySQL.EF.Models;
using System;

public class Stats
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