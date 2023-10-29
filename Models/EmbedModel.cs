using ShimmyMySherbet.MySQL.EF.Models;
using System;
using System.Runtime.Remoting.Messaging;

public class Embed
{
    public string WebhookURL { get; set; } = string.Empty;
    public int MessageID { get; set; }
    public string Header { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Field Field { get; set; } = new Field { Inline = true};


}