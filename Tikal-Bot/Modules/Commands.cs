using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tikal_Bot.Modules
{

    public class Commands : ModuleBase<SocketCommandContext>
    {

        [Command("ping")]
        public async Task PingAsync()
        {
            await ReplyAsync("pong! Hi, I'm here.");
        }


        [RequireUserPermission(GuildPermission.Administrator)]
        [Command("quit")]
        public async Task exitBot()
        {
            await ReplyAsync(":wave: See ya! \n");
            DiscordSocketClient task = new DiscordSocketClient();
            await task.StopAsync();
            await Task.Delay(500);
            Environment.Exit(0);
        }

        [Command("count")]
        public async Task CountDown(string count)
        {

            var conUser = Context.User;

            if (conUser is SocketGuildUser user)
            {
                //Order: admin, programmer, sadx crew, sa2 crew, etc.
                ulong[] roleID = new ulong[8] { 907298103365861427, 916759045371744387, 907297793100623933, 907297971266261003, 907298010902454272, 907298040212238378, 907298076648169502, 908011165085474897 };

                for (int i = 0; i < roleID.Length; i++)
                {
                    Console.WriteLine("\n id: %d", roleID[i]);
                    // Check if the user has the required role 
                    if (user.Roles.Any(r => r.Id == roleID[i]))
                    {
                        break;
                    }

                    if (i == roleID.Length -1)
                    {
                        await ReplyAsync("You don't have the permission for this action.");
                        return;
                    }
                }

                int numericValue;
                int numericCopy;

                bool isNumber = int.TryParse(count, out numericValue);


                if (count == "" || !isNumber || numericValue > 20)
                {
                    await ReplyAsync("Please enter a valid number, max allowed is 20. (ie: !count 10)");
                    return;
                }

                numericCopy = numericValue;
                await ReplyAsync(numericValue.ToString() + "....");
                numericValue--;

                do
                {

                    await Task.Delay(1120);

                    if (numericValue <= 5)
                    {
                        await Task.Delay(150);
                    }

                    int result = numericValue % 10;

                    if (result == 0 || result == 5 || result <= 4 && numericValue < 10)
                    {
                        if (numericValue == 0)
                            await ReplyAsync("GO!!!!");
                        else
                            await ReplyAsync(numericValue.ToString());
                    }

                    numericValue--;

                } while (numericValue > -1);
            }

            return;
        }

    }
}
