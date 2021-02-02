using System.Threading.Tasks;
using Discord.Commands;

namespace esp_discord_bot.src.discord {

    public class CommandsModule : ModuleBase<SocketCommandContext> {

        [Command("led-on")]
        public async Task LedON() {
            Program.server.SendToESP("ON");
            await ReplyAsync("LED is ON");
        }

        [Command("led-off")]
        public async Task LedOFF() {
            Program.server.SendToESP("OFF");
            await ReplyAsync("LED is OFF");
        }
    }
}