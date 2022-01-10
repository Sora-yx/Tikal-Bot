using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace Tikal_Bot
{
    class Program
    {
        private DiscordSocketClient client;
        private CommandService commands;
        public static List<string> chanList = new List<string>();

        static void Main(string[] args) => new Program().runBotAsync().GetAwaiter().GetResult();

        public async Task LogToDiscord()
        {
            try
            {
                using var sr = new StreamReader("info.txt");
                Console.WriteLine("Reading Discord token information...");
                string[] lines = File.ReadAllLines("info.txt");
                await client.LoginAsync(TokenType.Bot, lines[0]);
                sr.Close();
            }
            catch (IOException e)
            {
                Console.WriteLine("Error, couldn't read the file.");
                Console.WriteLine(e.Message);
                await client.StopAsync();
                await Task.Delay(1000);
                Environment.Exit(0);
            }
        }

        private async Task HandleCommandAsync(SocketMessage Pmsg)
        {
            var message = (SocketUserMessage)Pmsg; //Convert to Socket user msg

            if (message == null)
                return;

            int argPos = 0;

            if (!message.HasCharPrefix('!', ref argPos)) //if msg user doesn't start with a "!"
                return;

            var context = new SocketCommandContext(client, message);

            var result = await commands.ExecuteAsync(context, argPos, null);

            if (!result.IsSuccess)
                await context.Channel.SendMessageAsync(result.ErrorReason); //send error msg
        }

        public async Task InstallCommandsAsync()
        {
            client.MessageReceived += HandleCommandAsync;
            await commands.AddModulesAsync(Assembly.GetEntryAssembly(), null);
        }

        private void getChanList()
        {
            string txt = "chan.txt";

            try
            {
                using (var sr = new StreamReader(txt))
                {
                    Console.WriteLine("Reading Channels information...");
                    string[] lines = File.ReadAllLines(txt);
                    foreach (var curLine in lines)
                    {
                        chanList.Add(curLine);
                    }
                }
            }
            catch
            {
                Console.WriteLine("No chan.txt found.");
                return;
            }
        }

        private Task Log(LogMessage arg)
        {
            Console.WriteLine(arg.ToString());
            return Task.CompletedTask;
        }

        public async Task runBotAsync()
        {
            client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Debug
            });

            client.Log += Log;

            commands = new CommandService();


            await InstallCommandsAsync(); //set command users
            await LogToDiscord();
            getChanList();

            await client.StartAsync();
            await Task.Delay(-1);
        }

    }
}
