using System;
using BepInEx;
using DiscordRPC;

namespace Zeon
{
    [BepInPlugin("com.athena.discordrpc", "DiscordRPCClient", "1.0.0")]
    internal class DiscordBackend : BaseUnityPlugin
    {
        private DiscordRpcClient client;

        private void Awake()
        {
            client = new DiscordRpcClient("1303863454733439099");
            client.Initialize();

            SetRichPresence();
        }

        private void SetRichPresence()
        {
            client.SetPresence(new RichPresence()
            {
                Details = "Using Zeon Menu in Gorilla Tag!",
                State = "discord.gg/9ku2XQq7Wr",
                Assets = new DiscordRPC.Assets()
                {
                    LargeImageKey = "invertedlogo",
                    LargeImageText = "Gorilla Tag",
                    SmallImageKey = "Zeon",
                    SmallImageText = "Using Zeon Mod Menu"
                },
                Timestamps = new Timestamps()
                {
                    Start = DateTime.UtcNow
                },

                Buttons = new Button[]
                {
                    new Button { Label = "Get Zeon", Url = "https://discord.gg/9ku2XQq7Wr" }
                }
            });
        }

        private void OnDestroy()
        {
            client?.Dispose();
        }
    }
}
