﻿using OpenRPG.Utils;
using ProjectM;
using VampireCommandFramework;

namespace OpenRPG.Commands
{
    [CommandGroup(name: "kick")]
    public static class Kick
    {
        [Command(name: "player", shortHand: "p", adminOnly: false, usage: "<playername>", description: "Kick the specified player out of the server.")]
        public static void KickCommand(ChatCommandContext ctx, string name)
        {

            if (Helper.FindPlayer(name, true, out _, out var targetUserEntity))
            {
                Helper.KickPlayer(targetUserEntity);
                ctx.Reply($"Player \"{name}\" has been kicked from server.");
            }
            else
            {
                throw ctx.Error("Specified player not found.");
            }
        }
    }
}
