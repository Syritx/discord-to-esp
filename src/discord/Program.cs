using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

using esp_discord_bot.src.discord.config;
using esp_discord_bot.src.networking;

namespace esp_discord_bot.src.discord
{
    class Program
    {

        public static Server server;

        DiscordSocketClient client;
        CommandService commands;
        IServiceProvider services;

        string prefix, token;

        static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync() {

            server = new Server();

            prefix = Config.Prefix;
            token = Config.Token;

            client = new DiscordSocketClient();
            commands = new CommandService();

            services = new ServiceCollection()
                           .AddSingleton(client)
                           .AddSingleton(commands)
                           .BuildServiceProvider();

            client.Log += Log;

            await Commands();
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();
            await Task.Delay(-1);
        }

        public async Task Commands() {
            client.MessageReceived += CommandHandler;
            await commands.AddModulesAsync(System.Reflection.Assembly.GetEntryAssembly(), services);
        }

        Task Log(LogMessage msg) {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        async Task CommandHandler(SocketMessage message) {

            var msg = message as SocketUserMessage;
            var commandContext = new SocketCommandContext(client,msg);

            if (message.Author.IsBot) return;

            int argPos = 0;
            if (msg.HasStringPrefix(prefix, ref argPos))
            {
                var result = await commands.ExecuteAsync(commandContext, argPos, services);

                if (!result.IsSuccess) {
                    Console.WriteLine("there was an error: " + result.ErrorReason);
                }
            }
        }
    }
}
